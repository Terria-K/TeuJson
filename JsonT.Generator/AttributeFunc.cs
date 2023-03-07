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

    public static bool CheckIfDeserializable(ISymbol symbol, INamedTypeSymbol json) 
    {
        if (symbol is IPropertySymbol prop) 
        {
            if (prop.Type.Interfaces.Any(x => x.Name == "IJsonTDeserializable") || 
            prop.Type.GetAttributes().Any(x => x.AttributeClass?.Name == "JsonTSerializableAttribute"))
            {
                return true;
            }
        }
        else if (symbol is IFieldSymbol field) 
        {
            if (field.Type.Interfaces.Any(x => x.Name == "IJsoTnDeserializable") || 
            field.Type.GetAttributes().Any(x => x.AttributeClass?.Name == "JsonTSerializableAttribute"))
            {
                return true;
            }
        }

        return false;
    }
}