using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class IfNullAttribute : Attribute 
{
    public IfNullAttribute(IfNullOptions option) {}
}

public enum IfNullOptions 
{
    NullPersist,
    Ignore
}