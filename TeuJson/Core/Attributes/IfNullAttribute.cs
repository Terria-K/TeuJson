#pragma warning disable IDE0060
using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class IfNullAttribute : Attribute 
{
    public IfNullAttribute(IfNullOptions option) {}
}

public enum IfNullOptions 
{
    NullPersist,
    Ignore
}