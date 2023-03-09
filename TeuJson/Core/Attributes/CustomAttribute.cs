#pragma warning disable IDE0060
using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CustomAttribute : Attribute 
{
    public string? Write;
    public string? Read;

    public CustomAttribute(string converter) {}


    public CustomAttribute() {}
}