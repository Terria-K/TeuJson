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
        get 
        {
            if (Value.TryGetValue(key, out JsonValue? value))
                return value;
            return JsonNull.NullReference;
        }
        set => Value[key] = value; 
    }

    public override JsonValue this[int idx] 
    { 
        get         
        {
            if (Value.TryGetValue("x__" + idx, out JsonValue? value))
                return value;
            return JsonNull.NullReference;
        } 
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

    public override Dictionary<string, JsonValue> ToDictionary()
    {
        return Value;
    }

    public override JsonObject AsJsonObject => this;
}