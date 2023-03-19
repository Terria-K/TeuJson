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
    private static ulong tempCount;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var teuJsonProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax syntax
                    && syntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                    && syntax.BaseList?.Types.Count > 0,
                static (ctx, _) => 
                {
                    var jsonSyntax = (TypeDeclarationSyntax)ctx.Node;
                    var interfaces = ctx.SemanticModel.GetDeclaredSymbol(jsonSyntax)?.Interfaces;
                    if (interfaces is null)
                        return null;

                    foreach (var @interface in interfaces) 
                    {
                        var fullname = @interface.ToDisplayString();
                        if (fullname == "TeuJson.ISerialize" || fullname == "TeuJson.IDeserialize")
                            return jsonSyntax;
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
        

        foreach (var symbol in GetSymbols(comp, syn, "ISerialize"))
        {
            var members = symbol.GetMembers().OfType<ISymbol>().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("// Source Generated code");
            sb.AppendLine("using TeuJson;");
            sb.AppendLine("using System;");
            var ns = symbol.GetSymbolNamespace();
            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine("namespace " + ns + ";");

            GenerateSource(ctx, symbol, members, sb, true);
            ctx.AddSource($"{symbol.Name}.serialize.cs", sb.ToString());
        }

        foreach (var symbol in GetSymbols(comp, syn, "IDeserialize")) 
        {
            var members = symbol.GetMembers().OfType<ISymbol>().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("// Source Generated code");
            sb.AppendLine("using TeuJson;");
            sb.AppendLine("using System;");
            var ns = symbol.GetSymbolNamespace();
            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine("namespace " + ns + ";");

            GenerateSource(ctx, symbol, members, sb, false);
            ctx.AddSource($"{symbol.Name}.deserialize.cs", sb.ToString());
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
        var isRecord = false;
        if (classOrStruct == "record")
            isRecord = true;
        
        var hasBaseType = false;
        var isAbstract = symbol.IsAbstract;
        var isSealed = symbol.IsSealed;
        var baseType = symbol.BaseType;

        if (baseType != null)
        {
            var interfaces = baseType.Interfaces;
            var serializable = AttributeFunc.GetSerializeInterfaceData(interfaces);
            if (serializable != null)
            {
                hasBaseType = true;
            }
        }


        sb.AppendLine($"partial {classOrStruct} {symbol.Name}");
        sb.AppendLine("{");
        sb.AppendLine(AttributeFunc.GetStatusMethod(isSerialize, hasBaseType, isSealed, classOrStruct));

        sb.AppendLine("{");
        if (isSerialize)
            sb.AppendLine($"var __builder = {(hasBaseType ? "base.Serialize" : "new JsonObject")}();");
        if (hasBaseType && !isSerialize)
            sb.AppendLine("base.Deserialize(@__obj);");

        WriteMembers(ctx, members, sb, "this", isSerialize, isRecord);
        if (isSerialize)
            sb.AppendLine("return __builder;");
        sb.AppendLine("}");

        sb.AppendLine("}");
    }

    private static void WriteMembers(
        SourceProductionContext ctx, 
        List<ISymbol> members, 
        StringBuilder sb, 
        string qualifier,
        bool isSerialize,
        bool isRecord
    )
    {
        foreach (var sym in members)
        {
            if (isRecord && sym.Name == "EqualityContract")
                continue;
            if (sym is not IPropertySymbol && sym is not IFieldSymbol)
                continue;

            if (sym is IFieldSymbol && !sym.HasAttributeName("TeuObject"))
                continue;

            var name = $"{sym.Name}";
            var variableName = $"{qualifier}.{name}";
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
                            sb.AppendLine($"__builder[\"{name}\"] = {additionalCall}({variableName});");
                        else
                            sb.AppendLine($"{variableName} = {additionalCall}(@__obj[\"{name}\"]);");
                        goto Ignore;
                    }
                }
                if (attributeClassName == "IfNullAttribute")
                {
                    if (type is INamedTypeSymbol named)
                    {
                        if (named.ClassOrStruct() == "struct")
                            ctx.ReportDiagnostic(Diagnostic.Create(TeuDiagnostic.StructIfNull, sym.Locations[0]));
                    }

                    ifNull = AttributeFunc.IfNull(attr);
                }
            }

            // Collections
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
                            ctx.ReportDiagnostic(Diagnostic.Create(TeuDiagnostic.KeyTypeRule, sym.Locations[0]));
                            goto Ignore;
                        }

                        var typeName = typeSymbol.TypeArguments[1].ToDisplayString(NullableFlowState.None);
                        additionalCall = AttributeFunc.GetMethodToCallForDictionary(isSerialize, typeName);
                    }

                    else
                    {
                        ctx.ReportDiagnostic(Diagnostic.Create(TeuDiagnostic.ThreeArgumentsRule, sym.Locations[0]));
                        continue;
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

            // Check if the class has a TeuJsonSerializableAttribute
            if (type != null && AttributeFunc.CheckIfDeserializable(type, isSerialize))
            {
                if (isSerialize)
                {
                    if (type is INamedTypeSymbol named && named.ClassOrStruct() == "class")
                    {
                        sb.AppendLine($"if ({variableName} != null)");
                        sb.AppendLine($"__builder[\"{name}\"] = {variableName}.Serialize();");
                        if (ifNull)
                            continue;
                        sb.AppendLine($"else");
                        sb.AppendLine($"__builder[\"{name}\"] = new JsonNull();");
                    }
                    continue;
                }
                sb.AppendLine($"var @__temp{tempCount} = @__obj[\"{name}\"];");
                if (type is INamedTypeSymbol nameType && nameType.ClassOrStruct() == "struct")
                    sb.AppendLine($"{variableName} = default;");
                else
                    sb.AppendLine($"{variableName} = null;");
                sb.AppendLine($"if (!@__temp{tempCount}.IsNull)");
                sb.AppendLine("{");
                sb.AppendLine($"var result = new {type.ToDisplayString(NullableFlowState.None)}();");
                sb.AppendLine($"result.Deserialize(@__temp{tempCount}.AsJsonObject);");
                sb.AppendLine($"{variableName} = result;");
                sb.AppendLine("}");
                tempCount++;
                continue;
            }

            // Enums
            if (type != null && type.IsValueType && type is INamedTypeSymbol typedSymbol) 
            {
                var enumType = typedSymbol.EnumUnderlyingType;
                if (enumType != null) 
                {
                    if (isSerialize) 
                    {
                        sb.AppendLine(
                            $"__builder[\"{name}\"] = ({enumType.ToDisplayString()}){variableName}{additionalCall};"
                        );
                    }
                    else 
                    {
                        sb.AppendLine($"JsonValue @__enumTemp{tempCount} = @__obj[\"{name}\"]{additionalCall};");
                        sb.AppendLine($"if (System.Enum.TryParse(@__enumTemp{tempCount}.AsString, out {typedSymbol.Name} @t{tempCount}))");
                        sb.AppendLine("{");
                        sb.AppendLine($"{variableName} = @t{tempCount};");
                        sb.AppendLine("}");
                        sb.AppendLine("else");
                        sb.AppendLine("{");
                        sb.AppendLine(
                            $"{variableName} = ({typedSymbol.Name})({enumType.ToDisplayString()})@__enumTemp{tempCount++};");
                        sb.AppendLine("}");
                    }
                    continue;
                }
            }
            if (isSerialize) 
            {
                // Implicit Serializer
                sb.AppendLine($"__builder[\"{name}\"] = {variableName}{additionalCall};");
                continue;
            }
            // Implicit Deserializer
            sb.AppendLine($"{variableName} = @__obj[\"{name}\"]{additionalCall};");

            Ignore:
            sb.Append("");
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetSymbols(
        Compilation compilation, ImmutableArray<TypeDeclarationSyntax?> syn, string serialize) 
    {
        foreach (var partialClass in syn) 
        {
            if (partialClass is null)
                continue;
            var model = compilation.GetSemanticModel(partialClass.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(partialClass);
            if (symbol is null)
                continue;

            if (HasInterfaces(symbol, serialize))
                yield return symbol;
        }
    }

    private static bool HasAttributes(ISymbol symbol, string attributeName) => 
        symbol.GetAttributes().Any(attr => attr.AttributeClass!.Name.StartsWith(attributeName));

    private static bool HasInterfaces(INamedTypeSymbol symbol, string interfaceName) => 
        symbol.Interfaces.Any(intfa => intfa.Name.StartsWith(interfaceName));
}