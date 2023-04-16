#pragma warning disable IDE0060
using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
#if NET6_0
public class CustomAttribute : Attribute 
#else
public class CustomAttribute<T> : Attribute 
#endif
{
    public string? Write;
    public string? Read;

#if NET6_0
    public CustomAttribute(string converter) {}
#endif

    public CustomAttribute() {}
}