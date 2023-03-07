using System;

namespace JsonT.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class JsonTSerializableAttribute : Attribute {}
