using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CustomAttribute : Attribute 
{
    public CustomAttribute(string converter) 
    {

    }
}