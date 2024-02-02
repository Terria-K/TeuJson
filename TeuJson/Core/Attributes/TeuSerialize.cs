using System;

namespace TeuJson.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class TeuSerialize : Attribute {}