using System;
using System.Collections.Generic;
using System.Numerics;
using TeuJson;

namespace Utility;


public static partial class CustomConverters 
{
// I don't know how to do something like this [Custom(typeof(CustomConverters))]
// So, I just fallback into this one. I need help to understand the source generator more.
// There isn't much a helpful tips anywhere online, just a full of blogs about source generator.
    public const string Use = "Utility.CustomConverters";



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

    public static List<Vector2> ToList(this JsonValue value) 
    {
        var val = value.AsJsonArray;
        var vec2 = new List<Vector2>();
        foreach (JsonValue v in val) 
        {
            var x = v["x"];
            var y = v["y"];
            vec2.Add(new Vector2(x, y));
        }
        return vec2;
    }

    public static JsonArray ToJson(this List<Vector2> value) 
    {
        var jsonArray = new JsonArray();
        foreach (var val in value) 
        {
            jsonArray.Add(new JsonObject { ["X"] = val.X, ["Y"] = val.Y});
        }
        return jsonArray;
    }
}