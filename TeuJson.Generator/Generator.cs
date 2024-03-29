using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        

        foreach (var symbol in GetSerializableSymbols(comp, syn, "ISerialize"))
        {
            GenerateCode(symbol, true);
        }

        foreach (var symbol in GetSerializableSymbols(comp, syn, "IDeserialize")) 
        {
            GenerateCode(symbol, false);
        }

        void GenerateCode(INamedTypeSymbol symbol, bool serialize) 
        {
            var filename = serialize ? "serialize" : "deserialize";
            var members = symbol.GetMembers().OfType<ISymbol>().ToList();

            var ns = QuoteWriter.AddExpression(() => 
            {
                var ns = symbol.GetSymbolNamespace();
                if (string.IsNullOrEmpty(ns)) 
                {
                    return string.Empty;
                }
                return $"namespace {ns};";
            });

            var classOrStruct = symbol.ClassOrStruct();
            if (classOrStruct == "record" && !serialize) 
            {
                ctx.ReportDiagnostic(Diagnostic.Create(TeuDiagnostic.RecordRule, symbol.Locations[0]));
                return;
            }
            
            var hasBaseType = false;
            var isAbstract = symbol.IsAbstract;
            var isSealed = symbol.IsSealed;
            var baseType = symbol.BaseType;
            var isRecord = classOrStruct == "record";

            if (baseType != null)
            {
                var interfaces = baseType.Interfaces;
                var serializable = AttributeFunc.GetSerializeInterfaceData(interfaces);
                if (serializable != null)
                {
                    hasBaseType = true;
                }
            }
            var @class = QuoteWriter.AddExpression(() => classOrStruct);
            var classname = QuoteWriter.AddExpression(() => symbol.Name);
            var method = QuoteWriter.AddExpression(() => 
                AttributeFunc.GetStatusMethod(serialize, hasBaseType, isSealed, classOrStruct));
            var body = QuoteWriter.AddStatement(sb => 
            {
                if (serialize)
                    sb.AppendLine($"var __builder = {(hasBaseType ? "base.Serialize" : "new JsonObject")}();");
                if (hasBaseType && !serialize)
                    sb.AppendLine("base.Deserialize(@__obj);");

                WriteMembers(ctx, members, sb, "this", serialize, isRecord);
                if (serialize)
                    sb.AppendLine("return __builder;");
                return sb.ToString();
            });

            var quote = $$"""
            // Source Generated Code
            #nullable disable
            using TeuJson;
            using System;

            {{ns}}

            partial {{@class}} {{classname}}
            {
                public {{method}}
                {
            {{body}}
                }
            }
            """;
            ctx.AddSource($"{symbol.Name}.{filename}.cs", quote);
        }
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
            if (isRecord && sym.Name == "EqualityContract" || 
                sym is not IPropertySymbol && sym is not IFieldSymbol ||
                sym is IFieldSymbol && !sym.HasAttributeName("TeuObject")
            )
                continue;
            var name = $"{sym.Name}";
            var variableName = $"{qualifier}.{name}";
            ITypeSymbol? type = sym switch 
            {
                IPropertySymbol prop => prop.Type,
                IFieldSymbol field => field.Type,
                _ => null
            };

            var additionalCall = "";
            var ignoreStatement = "";
            bool ifNull = false;

            foreach (var attr in sym.GetAttributes())
            {
                var attributeClassName = attr.AttributeClass?.Name;
                if (attributeClassName is null)
                    continue;
                if (attributeClassName == "IgnoreAttribute") 
                {
                    var attrib = AttributeFunc.GetIgnoreCondition(attr);
                    if (attrib == string.Empty) 
                    {
                        // Idk how to get out of the 1st loop 
                        goto Ignore;
                    }
                    ignoreStatement = attrib;
                    sb.AppendLine($"if (!({ignoreStatement})) {{");
                }


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
            // Arrays
            else if (type is IArrayTypeSymbol arrayTypeSymbol)
            {
                var arrayName = arrayTypeSymbol.ToDisplayString(NullableFlowState.None);
                var elementType = arrayTypeSymbol.ElementType;
                if (elementType is INamedTypeSymbol namedTypeSymbol) 
                {
                    var underlyingType = namedTypeSymbol.EnumUnderlyingType;
                    if (underlyingType != null)
                    {
                        var fullDisplay = elementType.ToFullDisplayString();
                        if (isSerialize)
                            SerializeArrayEnum(sb, variableName, name, underlyingType, fullDisplay, ifNull);
                        else
                            DeserializeArrayEnum(sb, variableName, name, underlyingType, fullDisplay);

                        if (!string.IsNullOrEmpty(ignoreStatement))
                            sb.AppendLine("}");
                        continue;
                    }
                }
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
                        {
                            if (!string.IsNullOrEmpty(ignoreStatement))
                                sb.AppendLine("}");
                            continue;
                        }

                        sb.AppendLine($"else");
                        sb.AppendLine($"__builder[\"{name}\"] = new JsonNull();");
                        if (!string.IsNullOrEmpty(ignoreStatement))
                            sb.AppendLine("}");
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
                if (!string.IsNullOrEmpty(ignoreStatement))
                    sb.AppendLine("}");
                continue;
            }

            // Enums
            if (type != null && type.IsValueType && type is INamedTypeSymbol typedSymbol) 
            {
                var enumType = typedSymbol.EnumUnderlyingType;
                if (enumType != null) 
                {
                    var fullDisplay = typedSymbol.ToFullDisplayString();
                    if (isSerialize) 
                    {
                        sb.AppendLine(
                            $"__builder[\"{name}\"] = ({enumType.ToDisplayString()}){variableName}{additionalCall};"
                        );
                    }
                    else 
                    {
                        sb.AppendLine($"JsonValue @__enumTemp{tempCount} = @__obj[\"{name}\"]{additionalCall};");
                        sb.AppendLine($"if (System.Enum.TryParse(@__enumTemp{tempCount}.AsString, out {fullDisplay} @t{tempCount}))");
                        sb.AppendLine("{");
                        sb.AppendLine($"{variableName} = @t{tempCount};");
                        sb.AppendLine("}");
                        sb.AppendLine("else");
                        sb.AppendLine("{");
                        sb.AppendLine(
                            $"{variableName} = ({fullDisplay})({enumType.ToDisplayString()})@__enumTemp{tempCount++};");
                        sb.AppendLine("}");
                    }
                    if (!string.IsNullOrEmpty(ignoreStatement))
                        sb.AppendLine("}");
                    continue;
                }
            }
            if (isSerialize) 
            {
                // Implicit Serializer
                sb.AppendLine($"__builder[\"{name}\"] = {variableName}{additionalCall};");
                if (!string.IsNullOrEmpty(ignoreStatement))
                    sb.AppendLine("}");
                continue;
            }
            // Implicit Deserializer
            sb.AppendLine($"{variableName} = @__obj[\"{name}\"]{additionalCall};");
            if (!string.IsNullOrEmpty(ignoreStatement))
                sb.AppendLine("}");

            Ignore:
            sb.Append("");
        }
    }

    private static void DeserializeArrayEnum(
        StringBuilder sb, 
        string variableName,
        string name, 
        INamedTypeSymbol underlyingType, 
        string fullDisplay
    )
    {
        sb.AppendLine($$"""
        if (!@__obj.IsNull && @__obj.Count > 0)
        {
            var array = @__obj["{{name}}"].AsJsonArray;
            var arrayCount = array.Count; 
            {{variableName}} = new {{fullDisplay}}[arrayCount];
            for (int i = 0; i < arrayCount; i++) 
            {
                if (System.Enum.TryParse<{{fullDisplay}}>(array[i].AsString, out {{fullDisplay}} num)) 
                {
                    {{variableName}}[i] = num;
                    continue;
                }
                {{variableName}}[i] = ({{fullDisplay}})({{underlyingType.ToDisplayString()}})array[i];
            }
        }
        """);
    }

    private static void SerializeArrayEnum(
        StringBuilder sb,
        string variableName,
        string name, 
        INamedTypeSymbol underlyingType, 
        string fullDisplay,
        bool ifNull
    ) {
        if (!ifNull) 
        {
            sb.AppendLine($"if ({variableName} == null)");
            sb.AppendLine("{");
            sb.AppendLine($"__builder[\"{name}\"] = new JsonArray();");
            sb.AppendLine("}");
            sb.AppendLine("else");
        }
        else 
        {
            sb.AppendLine($"if ({variableName} != null)");
        }
        sb.AppendLine($$"""
        {
            var jsonArray = new JsonArray();
            foreach ({{fullDisplay}} v in {{variableName}}) 
            {
                jsonArray.Add(({{underlyingType.ToDisplayString()}})v);
            }
            __builder["{{name}}"] = jsonArray;
        }
        """);
    }

    private static IEnumerable<INamedTypeSymbol> GetSerializableSymbols(
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

    private static bool HasInterfaces(INamedTypeSymbol symbol, string interfaceName) => 
        symbol.Interfaces.Any(intfa => intfa.Name.StartsWith(interfaceName));
}