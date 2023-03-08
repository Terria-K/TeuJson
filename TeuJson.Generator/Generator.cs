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
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeProvider = context.SyntaxProvider
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
                            if (fullname == "TeuJson.Attributes.TeuJsonSerializableAttribute.TeuJsonSerializableAttribute()")
                                return jsonSyntax;
                        }
                    }
                    return null;
                }
            ).Where(m => m is not null);

        var compilation = context.CompilationProvider.Combine(typeProvider.Collect());

        context.RegisterSourceOutput(
            compilation, static (ctx, source) => Generate(ctx, source.Left, source.Right)
        );
    }

    private static void Generate(SourceProductionContext ctx, Compilation comp, ImmutableArray<TypeDeclarationSyntax?> syn) 
    {
        if (syn.IsDefaultOrEmpty)
            return;
        var attribute = comp.GetTypeByMetadataName("TeuJson.Attributes.TeuJsonSerializableAttribute");

        if (attribute is null) 
            return;
        

        foreach (var symbol in GetSymbols(comp, syn))
        {
            var data = symbol.GetAttributes()[0];
            var option = AttributeFunc.GetOptions(data);
            var members = symbol.GetMembers().OfType<ISymbol>().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("// Source Generated code");
            sb.AppendLine("using TeuJson;");
            var ns = symbol.GetSymbolNamespace();
            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine("namespace " + ns + ";");

            if (option.Deserializable)
                GenerateSource(ctx, attribute, symbol, members, sb, false);
            if (option.Serializable) 
                GenerateSource(ctx, attribute, symbol, members, sb, true);
            ctx.AddSource($"{symbol.Name}.g.cs", sb.ToString());
        }
    }

    private static void GenerateSource(
        SourceProductionContext ctx, 
        INamedTypeSymbol? attribute, 
        INamedTypeSymbol symbol, 
        List<ISymbol> members,
        StringBuilder sb,
        bool isSerialize 
    )
    {
        sb.AppendLine($"partial {symbol.ClassOrStruct()} {symbol.Name} : {AttributeFunc.GetStatusInterface(isSerialize)}");
        sb.AppendLine("{");
        sb.AppendLine(AttributeFunc.GetStatusMethod(isSerialize));
        sb.AppendLine("{");
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
                    name = AttributeFunc.TName(name, attr);
                }
                if (attributeClassName == "CustomAttribute") 
                {
                    additionalCall = AttributeFunc.GetCustomConverter(isSerialize, type?.Name);
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
                        additionalCall = ListCheck(typeArguments[0].Name, isSerialize);
                    else if (typeArguments.Length == 2)
                        additionalCall = AttributeFunc.GetMethodToCallForDictionary(isSerialize);
                    else
                        throw new Exception("Three type arguments is not supported!");
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

            if (AttributeFunc.CheckIfDeserializable(sym, attribute, isSerialize))
            {
                additionalCall = AttributeFunc.GetMethodToCall(isSerialize);
            }
            if (isSerialize)
                sb.AppendLine($"[\"{name}\"] = {sym.Name}{additionalCall},");
            else
                sb.AppendLine($"{sym.Name} = obj[\"{name}\"]{additionalCall};");
            Ignore:
            sb.Append("");
        }
        sb.AppendLine(AttributeFunc.GetFunctionEnd(isSerialize));
        sb.AppendLine("}");
    }

    private static IEnumerable<INamedTypeSymbol> GetSymbols(
        Compilation compilation, ImmutableArray<TypeDeclarationSyntax?> syn) 
    {
        foreach (var partialClass in syn) 
        {
            if (partialClass is null)
                continue;
            var model = compilation.GetSemanticModel(partialClass.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(partialClass);
            if (symbol is null)
                continue;

            if (HasAttributes(symbol, "TeuJsonSerializable"))
                yield return symbol;
        }
    }

    private static bool HasAttributes(ISymbol symbol, string attributeName) => 
        symbol.GetAttributes().Any(attr => attr.AttributeClass!.Name.StartsWith(attributeName));
}