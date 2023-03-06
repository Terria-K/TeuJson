using System;
using System.Collections.Generic;
using System.Text;

namespace JsonT;

public sealed class JsonObject : JsonValue<Dictionary<string, JsonValue>>
{
    public JsonObject() : base(JsonToken.Object, new Dictionary<string, JsonValue>())
    {
    }

    public override JsonValue this[string key] 
    { 
        get => Value[key]; 
        set => Value[key] = value; 
    }

    public override JsonValue this[int idx] 
    { 
        get => Value["x__" + idx]; 
        set => Value["x__" + idx] = value; 
    }

    public override int Count => Value.Count;
    public override IEnumerable<string> Keys => Value.Keys;
    public override IEnumerable<JsonValue> Values => Value.Values;
    public override IEnumerable<KeyValuePair<string, JsonValue>> Pairs => Value;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("{");
        foreach (var obj in Value) 
        {
            sb.AppendLine($"\"{obj.Key}\": {obj.Value.ToString()}");
        }
        sb.AppendLine("}");
        return sb.ToString();
    }
}