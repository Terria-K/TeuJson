using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace TeuJson.Generator;

internal static class AttributeFunc 
{
    public static string TName(AttributeData data) 
    {
        string final = "";
        if (!data.ConstructorArguments.IsEmpty) 
        {
            var args = data.ConstructorArguments;
            if (args.Length == 1) 
            {
                var arg = (string?)args[0].Value;
                if (arg is not null)
                    final = arg;
            }
        }
        if (!data.NamedArguments.IsEmpty) 
        {
            var args = data.NamedArguments;
            foreach (var arg in args) 
            {
                var typedConstant = arg.Value.Value;
                if (arg.Key == "PropertyName" && typedConstant is not null) 
                {
                    final = (string)typedConstant;
                }
            }
        }
        return final;
    }

    public static bool JDictionary(AttributeData attr) 
    {
        if (!attr.NamedArguments.IsEmpty) 
        {
            var args = attr.NamedArguments;
            foreach (var arg in args) 
            {
                var typedConstant = arg.Value.Value;
                if (arg.Key == "Dynamic" && typedConstant is not null)
                    return (bool)typedConstant;
            }
        }
        return false;
    }

    public static bool CheckIfDeserializable(ITypeSymbol type, bool isSerialize)
    {
        var interfaceToCheck = GetStatusInterface(isSerialize);
        return type.Interfaces.Any(x => x.Name == interfaceToCheck);
    }

    public static JsonOptions GetOptions(SourceProductionContext ctx, INamedTypeSymbol symbol, AttributeData data) 
    {
        bool serializable = false;
        bool deserializable = false;
        if (!data.NamedArguments.IsEmpty) 
        {
            var args = data.NamedArguments;
            foreach (var arg in args) 
            {
                var typedConstant = arg.Value.Value;
                if (typedConstant is null)
                    continue;
                if (arg.Key == "Serializable") 
                    serializable = (bool)typedConstant;
                else if (arg.Key == "Deserializable") 
                    deserializable = (bool)typedConstant;
                
            }
        }
        if (!serializable && !deserializable)
            ctx.ReportDiagnostic(Diagnostic.Create(TeuDiagnostic.WillNotWork, symbol.Locations[0]));
        return new JsonOptions 
        {
            Serializable = serializable,
            Deserializable = deserializable
        };
    }

    public static string GetIgnoreCondition(AttributeData data) 
    {
        if (!data.ConstructorArguments.IsEmpty) 
        {
            var arg = data.ConstructorArguments[0];
            var typedConstant = arg.Value;
            if (typedConstant != null) 
            {
                return (string)typedConstant;
            }
        }
        return string.Empty;
    }

    public static (bool, string) GetCustomConverter(bool serializable, string? typeName, AttributeData data) 
    {
        var directCall = false;
        var converter = string.Empty;
        if (!data.ConstructorArguments.IsEmpty) 
        {
            var arg = data.ConstructorArguments[0];
            
            var typedConstant = arg.Value;
            if (typedConstant != null) 
            {
                directCall = true;
                var converterType = (string)typedConstant;
                converter = converterType;
            }
            string? write = string.Empty;
            string? read = string.Empty;
            if (!data.NamedArguments.IsEmpty) 
            {
                var args = data.NamedArguments;
                foreach (var argument in args) 
                {
                    var typedConst = argument.Value;
                    if (argument.Key == "Write")
                        write = (string?)typedConst.Value;
                    else if (argument.Key == "Read")
                        read = (string?)typedConst.Value;
                }
            }


            if (serializable) 
            {
                if (string.IsNullOrEmpty(write))
                    return (directCall, $"{converter}.ToJson");
                return (directCall, $"{converter}.{write}");
            }
            if (!string.IsNullOrEmpty(read))
                return (directCall, $"{converter}.{read}");
            return (directCall, $"{converter}.To{typeName}");
        }
        if (serializable)
            return (directCall, $".ToJson()");
        return (directCall, $".To{typeName}()");
    }

    public static string GetStatusInterface(bool serializable) 
    {
        if (serializable)
            return "ISerialize";
        return "IDeserialize";
    }

    public static string GetStatusMethod(bool serializable, bool hasBaseType, bool isSealed, string classOrStruct) 
    {
        if (classOrStruct == "struct") 
        {
            if (serializable)
                return $"JsonObject Serialize()";
            return $"void Deserialize(JsonObject @__obj)";
        }
        if (serializable)
            return $"{(hasBaseType ? $"{(isSealed ? "sealed" : "")} override" : $"{GetStatusMethodModifier(isSealed)}")} JsonObject Serialize()";
        return $"{(hasBaseType ? $"{(isSealed ? "sealed" : "")} override" : GetStatusMethodModifier(isSealed))} void Deserialize(JsonObject @__obj)";
    }

    private static string GetStatusMethodModifier(bool isSealed) 
    {
        if (isSealed)
            return "";
        return "virtual";
    }

    public static string GetMethodToCall(bool serializable, string symbolName) 
    {
        if (serializable)
            return ".Serialize()";
        return $".Convert<{symbolName}>()";
    }

    public static string GetMethodToCallForDictionary(bool serializable, string symbolName) 
    {

        if (serializable)
            return ".ToJsonObject()";
        if (symbolName == "TeuJson.JsonValue")
            return $".AsDictionary()";
        return $".ToDictionary<{symbolName}>()";
    }

    public static bool IfNull(AttributeData data) 
    {
        if (!data.ConstructorArguments.IsEmpty) 
        {
            var value = data.ConstructorArguments[0].Value;
            if (value != null) 
            {
                var val = (IfNullOptions)value;
                return val switch 
                {
                    IfNullOptions.Ignore => true,
                    IfNullOptions.NullPersist => false,
                    _ => true
                };
            }
        }
        return true;
    }

    public static INamedTypeSymbol? GetSerializeInterfaceData(ImmutableArray<INamedTypeSymbol> datas)
    {
        foreach (var data in datas) 
        {
            if (data?.Name == "ISerialize" || data?.Name == "IDeserialize") 
            {
                return data;
            }
        }
        return null;
    } 
}

internal struct JsonOptions 
{
    public bool Serializable;
    public bool Deserializable;
}

internal enum IfNullOptions 
{
    NullPersist,
    Ignore
}