using System;
using System.Collections.Generic;
using System.Numerics;
using TeuJson;
using TeuJson.Attributes;
using Utility;

namespace Structural;

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Structure 
{
    [Name("array")]
    public int[]? Array { get; set; }
    public int Number { get; set; }
    public string? Text { get; set; }
    [Name("falsy")]
    public bool TrueOrFalse { get; set; }
    [Name("otherName")]
    [TeuObject]
    public string? Field;
    // Local Converters.
    [Custom]
    public TimeSpan Span { get; set; }

    // Converters outside of a namespace of a structure class need to use the fully qualified name of a converter class.
    [Custom("Utility.CustomConverters")]
    public Vector2 Position { get; set; }
    // Constant should work too!
    [Custom(CustomConverters.Use)]
    public List<Vector2>? Positions { get; set; }

    [Custom(CustomConverters.Use, Write = "CustomizedBooleanWriter", Read = "CustomizedBoolean")]
    public bool IsNull { get; set; }

    public int[,]? Array2D { get; set; }
    public EmptyStructure[]? Structures { get; set; }
    public List<EmptyStructure>? StructureList { get; set; }
    public EmptyStructure StructureObject { get; set; }
    public Dictionary<string, EmptyStructure>? ValueDictionary { get; set; }
    public Dictionary<string, JsonValue>? Values { get; set; }
    public JsonValue? RawValue { get; set; }

    [IfNull(IfNullOptions.NullPersist)]
    public NullishStructure? NullableStructure { get; set; }


    [IfNull(IfNullOptions.NullPersist)]
    public int DefaultValue;

    [IfNull(IfNullOptions.Ignore)]
    [TeuObject]
    public NullishStructure? IgnoreMe;

    [TeuObject]
    public Vector3? Vec3;

    [TeuObject]
    public Vector4? Vec4;

    [TeuObject]
    public Enumeration NumberEnum;
    [TeuObject]
    public Enumeration TextEnum = Enumeration.C;
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial struct EmptyStructure {}
[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class NullishStructure {}

[TeuJsonSerializable]
public partial class TestWillNotWork {}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public sealed partial class Rect4 
{ 
    public Vector2X? Width { get; set; }
    public Vector2X? Height { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Vector1 
{
    public float X { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public sealed partial class Vector2X : Vector1 
{
    public float Y { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Vector3 : Vector1 
{
    public float Y { get; set; }
    public float Z { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Vector4 : Vector3 
{
    public float W { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public abstract partial class AbstractVector 
{
    public float X { get; set; }
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public sealed partial class Vectorized: AbstractVector
{
    public float Hello { get; set; }
}

public static class LocalConverter 
{
    public static TimeSpan ToTimeSpan(this JsonValue value) 
    {
        if (value.IsNumber)
            return TimeSpan.FromTicks(value.AsInt64);
        return TimeSpan.Zero;
    }

    public static JsonValue ToJson(this TimeSpan value) 
    {
        var ticks = value.Ticks;
        return new JsonValue<long>(JsonToken.Number, ticks);
    }
}

public enum Enumeration : byte
{
    A,
    B,
    C = 4
}