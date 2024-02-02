using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class TeuObjectAttribute : Attribute {}