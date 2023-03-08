using Microsoft.CodeAnalysis;
using System.Linq;

namespace JsonT;

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

    public static bool CheckIfDeserializable(ISymbol symbol, INamedTypeSymbol? json, bool isSerialize) 
    {
        var interfaceToCheck = GetStatusInterface(isSerialize);
        var interfaceAttribute = interfaceToCheck + "Attribute";
        if (symbol is IPropertySymbol prop) 
        {
            if (prop.Type.Interfaces.Any(x => x.Name == interfaceToCheck) || 
            prop.Type.GetAttributes().Any(x => x.AttributeClass?.Name == interfaceAttribute))
            {
                return true;
            }
        }
        else if (symbol is IFieldSymbol field) 
        {
            if (field.Type.Interfaces.Any(x => x.Name == interfaceToCheck) || 
            field.Type.GetAttributes().Any(x => x.AttributeClass?.Name == interfaceAttribute))
            {
                return true;
            }
        }

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

    public static string GetCustomConverter(bool serializable, string? typeName) 
    {
        if (serializable)
            return $".{typeName}ToJson()";
        return $".ConvertTo{typeName}()";
    }

    public static string GetStatusInterface(bool serializable) 
    {
        if (serializable)
            return "IJsonTSerializable";
        return "IJsonTDeserializable";
    }

    public static string GetStatusMethod(bool serializable) 
    {
        if (serializable)
            return "public JsonObject Serialize() => new JsonObject";
        return "public void Deserialize(JsonObject obj)";
    }

    public static string GetFunctionEnd(bool serializable) 
    {
        if (serializable)
            return "};";
        return "}";
    }

    public static string GetMethodToCall(bool serializable) 
    {
        if (serializable)
            return ".Serialize()";
        return ".Deserialize(obj)";
    }

    public static string GetMethodToCallForDictionary(bool serializable) 
    {
        if (serializable)
            return ".ToJsonObject()";
        return ".ToDictionary()";
    }
}

public struct JsonOptions 
{
    public bool Serializable;
    public bool Deserializable;
}