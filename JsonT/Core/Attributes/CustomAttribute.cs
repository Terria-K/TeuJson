using System;

namespace JsonT.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CustomAttribute : Attribute {}