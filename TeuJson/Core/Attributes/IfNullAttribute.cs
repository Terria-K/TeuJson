using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class IfNullAttribute : Attribute 
{
#pragma warning disable IDE0060
    public IfNullAttribute(IfNullOptions option) {}
#pragma warning restore IDE0060
}

public enum IfNullOptions 
{
    NullPersist,
    Ignore
}