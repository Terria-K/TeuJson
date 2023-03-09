using System.Linq;
using Microsoft.CodeAnalysis;

namespace TeuJson.Generator;

internal static class RoslynExtensions 
{
    public static string ToFullDisplayString(this ISymbol s) 
    {
        return s.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    public static string GetSymbolNamespace(this INamedTypeSymbol symbol) 
    {
        string namespaceName = string.Empty;
        INamespaceSymbol ns = symbol.ContainingNamespace;
        while (ns?.ContainingNamespace is { } containing) 
        {
            if (namespaceName is null) 
            {
                namespaceName = ns.Name;
                ns = containing;
                continue;
            }
            namespaceName = ns.Name + "." + namespaceName;
            ns = containing;
        }
        if (namespaceName.Length > 0)
            return namespaceName.Remove(namespaceName.Length - 1);
        return namespaceName;
    }

    public static bool IsOfBaseType(this ITypeSymbol type,ITypeSymbol baseType) 
    {
        while (type is not null) 
        {
            if (SymbolEqualityComparer.Default.Equals(type, baseType)) 
            {
                return true;
            }
            if (type.BaseType is null)
                continue;
            type = type.BaseType;
        }
        return false;
    }

    public static bool HasAttributeName(this ISymbol symbol, string attributeName) 
    {
        if (symbol.GetAttributes().Any(attr => attr.AttributeClass!.Name.StartsWith(attributeName))) 
        {
            return true;
        }
        return false;
    }

    public static string ClassOrStruct(this INamedTypeSymbol symbol) 
    {
        if (symbol.IsValueType)
            return "struct";
        else if (symbol.IsRecord)
            return "record";
        return "class";
    }
}