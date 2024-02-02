#pragma warning disable IDE0060
using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
#if !NET7_0_OR_GREATER
public class CustomAttribute : Attribute 
#else
public class CustomAttribute<T> : Attribute 
#endif
{
    public string? Write;
    public string? Read;

#if !NET7_0_OR_GREATER
    public CustomAttribute(string converter) {}
#endif

    public CustomAttribute() {}
}