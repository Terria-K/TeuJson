using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class IgnoreAttribute : Attribute 
{
    public IgnoreAttribute() {}
    public IgnoreAttribute(string condition) {}
}
