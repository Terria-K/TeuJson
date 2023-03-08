using System;
using TeuJson;
using TeuJson.Attributes;

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
    [Custom]
    public TimeSpan Span { get; set; }
    [Custom]
    public Vector2 Position { get; set; }
}

public static class CustomConverters 
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

    public static Vector2 ToVector2(this JsonValue value) 
    {
        if (value.IsObject) 
        {
            int x = value["x"];
            int y = value["y"];
            return new Vector2(x, y);
        }
        return new Vector2(0, 0);
    }

    public static JsonValue ToJson(this Vector2 value) 
    {
        return new JsonObject 
        {
            ["x"] = value.X,
            ["y"] = value.Y
        };
    }
}

public struct Vector2 
{
    public int X;
    public int Y;

    public Vector2(int x, int y) 
    {
        X = x;
        Y = y;
    }
}