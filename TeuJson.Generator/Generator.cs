using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TeuJson.Generator;

[Generator]
public sealed partial class TeuJsonGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor ThreeArgumentsRule 
        = new("TE001", "Three type arguments are not supported.", 
            "Three type arguments are not supported", "Usage", DiagnosticSeverity.Error, true, "Use one type argument to fix this error.");

    private static readonly DiagnosticDescriptor KeyTypeRule
        = new("TE002", "Key types are not supported.", 
            "Key types other than string are not supported", "Usage", DiagnosticSeverity.Error, true, "Use string as a key type instead.");

    private static readonly DiagnosticDescriptor RecordRule
        = new("TE004", "Record is not yet supported.", 
            "Record is not yet supported, but it will come in the future.", 
            "Usage", DiagnosticSeverity.Warning, true, "Mark it as class or struct.");

    private static readonly DiagnosticDescriptor StructIfNull
        = new("TE005", "Struct type does not need to have [IfNull] Atrribute", 
            "Structs are not nullable, it doesn't makes sense to have this attribute.", 
            "Usage", DiagnosticSeverity.Warning, true, "Remove the [IfNull] Attribute");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var teuJsonProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax syntax
                    && syntax.AttributeLists.Count > 0
                    && syntax.Modifiers.Any(SyntaxKind.PartialKeyword),
                static (ctx, _) => 
                {
                    var jsonSyntax = (TypeDeclarationSyntax)ctx.Node;
                    foreach (var attribListSyntax in jsonSyntax.AttributeLists) 
                    {
                        foreach (var attribSyntax in attribListSyntax.Attributes) 
                        {
                            if (ctx.SemanticModel.GetSymbolInfo(attribSyntax).Symbol is not IMethodSymbol attribSymbol)
                                continue;
                            
                            var namedTypeSymbol = attribSymbol.ContainingType;
                            var fullname = attribSymbol.ToDisplayString();

                            if (fullname == 
                            "TeuJson.Attributes.TeuJsonSerializableAttribute.TeuJsonSerializableAttribute()")
                                return jsonSyntax;
                        }
                    }
                    return null;
                }
            ).Where(m => m is not null);
        
        var teuJsonCompilation = context.CompilationProvider.Combine(teuJsonProvider.Collect());

        context.RegisterSourceOutput(
            teuJsonCompilation, static (ctx, source) => Generate(ctx, source.Left, source.Right)
        );
    }

    private static void Generate(SourceProductionContext ctx, Compilation comp, ImmutableArray<TypeDeclarationSyntax?> syn) 
    {
        if (syn.IsDefaultOrEmpty)
            return;
        var attribute = comp.GetTypeByMetadataName("TeuJson.Attributes.TeuJsonSerializableAttribute");

        if (attribute is null)  
            return;

        foreach (var symbol in GetSymbols(comp, syn, "TeuJsonSerializable"))
        {
            var data = symbol.GetAttributes()[0];
            var option = AttributeFunc.GetOptions(ctx, symbol, data);
            if (!option.Deserializable && !option.Serializable)
                continue;
            var members = symbol.GetMembers().OfType<ISymbol>().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("// Source Generated code");
            sb.AppendLine("using TeuJson;");
            var ns = symbol.GetSymbolNamespace();
            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine("namespace " + ns + ";");

            if (option.Deserializable)
                GenerateSource(ctx, symbol, members, sb, false);
            if (option.Serializable) 
                GenerateSource(ctx, symbol, members, sb, true);
            ctx.AddSource($"{symbol.Name}.g.cs", sb.ToString());
        }
    }

    private static void GenerateSource(
        SourceProductionContext ctx, 
        INamedTypeSymbol symbol, 
        List<ISymbol> members,
        StringBuilder sb,
        bool isSerialize 
    )
    {
        var classOrStruct = symbol.ClassOrStruct();
        if (classOrStruct == "record") 
        {
            ctx.ReportDiagnostic(Diagnostic.Create(RecordRule, symbol.Locations[0]));
            return;
        }

        sb.AppendLine($"partial {symbol.ClassOrStruct()} {symbol.Name} : {AttributeFunc.GetStatusInterface(isSerialize)}");
        sb.AppendLine("{");
        sb.AppendLine(AttributeFunc.GetStatusMethod(isSerialize));
        sb.AppendLine("{");
        if (isSerialize) 
            sb.AppendLine("var __builder = new JsonObject();");
        foreach (var sym in members)
        {
            if (sym is not IPropertySymbol && sym is not IFieldSymbol)
                continue;

            if (sym is IFieldSymbol && !sym.HasAttributeName("TeuObject"))
                continue;

            var name = sym.Name;
            ITypeSymbol? type;
            if (sym is IPropertySymbol prop)
                type = prop.Type;
            else if (sym is IFieldSymbol field)
                type = field.Type;
            else type = null;

            var additionalCall = "";
            bool ifNull = false;

            foreach (var attr in sym.GetAttributes())
            {
                var attributeClassName = attr.AttributeClass?.Name;
                if (attributeClassName is null)
                    continue;
                if (attributeClassName == "IgnoreAttribute")
                    // Idk how to get out of the 1st loop 
                    goto Ignore;

                if (attributeClassName == "NameAttribute")
                {
                    name = AttributeFunc.TName(attr);
                }
                if (attributeClassName == "CustomAttribute") 
                {
                    bool directCall;
                    (directCall, additionalCall) = AttributeFunc.GetCustomConverter(isSerialize, type?.Name, attr);
                    if (directCall) 
                    {
                        if (isSerialize)
                            sb.AppendLine($"__builder[\"{name}\"] = {additionalCall}({sym.Name});");
                        else
                            sb.AppendLine($"{sym.Name} = {additionalCall}(obj[\"{name}\"]);");
                        goto Ignore;
                    }
                }
                if (attributeClassName == "IfNullAttribute") 
                {
                    if (type is INamedTypeSymbol named) 
                    {
                        if (named.ClassOrStruct() == "struct")
                            ctx.ReportDiagnostic(Diagnostic.Create(StructIfNull, sym.Locations[0]));
                    }
                        
                    ifNull = AttributeFunc.IfNull(attr);
                }
            }
            if (type is not null && type is INamedTypeSymbol typeSymbol)
            {
                var typeArguments = typeSymbol.TypeArguments;
                // Check if it has type arguments for list and dictionaries
                // if it exceed to three, break it out
                if (typeArguments.Length > 0 && typeArguments.Length < 3)
                {
                    // List
                    if (typeArguments.Length == 1)
                        additionalCall = ListCheck(typeArguments[0].ToDisplayString(NullableFlowState.None), isSerialize);
                    else if (typeArguments.Length == 2) 
                    {
                        if (typeSymbol.TypeArguments[0].Name != "String") 
                        {
                            ctx.ReportDiagnostic(Diagnostic.Create(KeyTypeRule, sym.Locations[0]));
                            goto Ignore;
                        }

                        var typeName = typeSymbol.TypeArguments[1].ToDisplayString(NullableFlowState.None);
                        additionalCall = AttributeFunc.GetMethodToCallForDictionary(isSerialize, typeName);
                    }

                    else
                    {
                        ctx.ReportDiagnostic(Diagnostic.Create(ThreeArgumentsRule, sym.Locations[0]));
                        goto Ignore;
                    }

                }
            }
            else if (type is IArrayTypeSymbol arrayTypeSymbol)
            {
                var arrayName = arrayTypeSymbol.ToDisplayString(NullableFlowState.None);
                if (arrayTypeSymbol.Rank == 1)
                    additionalCall = ArrayCheck(arrayName, isSerialize);
                else
                    additionalCall = Array2DCheck(arrayName, isSerialize);
            }

            if (AttributeFunc.CheckIfDeserializable(type, isSerialize, out string n))
            {
                additionalCall = AttributeFunc.GetMethodToCall(isSerialize, n);
                if (isSerialize) 
                {
                    if (type is INamedTypeSymbol named && named.ClassOrStruct() == "class") 
                    {
                        sb.AppendLine($"if ({sym.Name} != null)");
                        sb.AppendLine($"__builder[\"{name}\"] = {sym.Name}{additionalCall};");
                        if (ifNull)
                            goto Ignore;
                        sb.AppendLine($"else");
                        sb.AppendLine($"__builder[\"{name}\"] = new JsonNull();");
                    }
                    goto Ignore;
                }

            }
            if (isSerialize)
                sb.AppendLine($"__builder[\"{name}\"] = {sym.Name}{additionalCall};");
            else
                sb.AppendLine($"{sym.Name} = obj[\"{name}\"]{additionalCall};");
            Ignore:
            sb.Append("");
        }
        if (isSerialize)
            sb.AppendLine("return __builder;");
        sb.AppendLine("}");
        sb.AppendLine("}");
    }

    private static IEnumerable<INamedTypeSymbol> GetSymbols(
        Compilation compilation, ImmutableArray<TypeDeclarationSyntax?> syn, string attribute) 
    {
        foreach (var partialClass in syn) 
        {
            if (partialClass is null)
                continue;
            var model = compilation.GetSemanticModel(partialClass.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(partialClass);
            if (symbol is null)
                continue;

            if (HasAttributes(symbol, attribute))
                yield return symbol;
        }
    }

    private static bool HasAttributes(ISymbol symbol, string attributeName) => 
        symbol.GetAttributes().Any(attr => attr.AttributeClass!.Name.StartsWith(attributeName));
}