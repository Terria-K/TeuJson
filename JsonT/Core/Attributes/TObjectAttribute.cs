using System;

namespace JsonT.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TObjectAttribute : Attribute {}