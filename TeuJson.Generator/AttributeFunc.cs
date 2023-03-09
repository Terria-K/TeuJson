using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace TeuJson.Generator;

public static class AttributeFunc 
{
    public static string TName(string name, AttributeData data) 
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

    public static bool CheckIfDeserializable(ISymbol symbol, INamedTypeSymbol? json, bool isSerialize, out string name) 
    {
        var interfaceToCheck = GetStatusInterface(isSerialize);
        var interfaceAttribute = "TeuJsonSerializableAttribute";
        if (symbol is IPropertySymbol prop) 
        {
            if (prop.Type.Interfaces.Any(x => x.Name == interfaceToCheck) || 
            prop.Type.GetAttributes().Any(x => x.AttributeClass?.Name == interfaceAttribute))
            {
                name = prop.Type.ToDisplayString(NullableFlowState.None);
                return true;
            }
        }
        else if (symbol is IFieldSymbol field) 
        {
            if (field.Type.Interfaces.Any(x => x.Name == interfaceToCheck) || 
            field.Type.GetAttributes().Any(x => x.AttributeClass?.Name == interfaceAttribute))
            {
                name = field.Type.ToDisplayString(NullableFlowState.None);
                return true;
            }
        }
        name = "Unknown";
        return false;
    }

    public static JsonOptions GetOptions(AttributeData data) 
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
        return new JsonOptions 
        {
            Serializable = serializable,
            Deserializable = deserializable
        };
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
            return "ITeuJsonSerializable";
        return "ITeuJsonDeserializable";
    }

    public static string GetStatusMethod(bool serializable) 
    {
        if (serializable)
            return "public JsonObject Serialize()";
        return "public void Deserialize(JsonObject obj)";
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
                    IfNullOptions.Ignore => false,
                    IfNullOptions.NullPersist => true,
                    _ => false
                };
            }
        }
        return false;
    }
}

public struct JsonOptions 
{
    public bool Serializable;
    public bool Deserializable;
}

public struct DefaultImplementationOptions 
{
    public bool Ignore;
    public string Call;
}

public enum IfNullOptions 
{
    NullPersist,
    Ignore
}