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
}

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial struct EmptyStructure {}

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