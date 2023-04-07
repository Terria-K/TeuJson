using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class IgnoreAttribute : Attribute 
{
    public IgnoreAttribute() {}
    public IgnoreAttribute(string condition) {}
}
