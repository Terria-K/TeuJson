using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NameAttribute : Attribute 
{
    public string JsonName { get; set; }

    public NameAttribute(string name) 
    {
        JsonName = name;
    }
}
