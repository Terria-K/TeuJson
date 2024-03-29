using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TeuJson;

public sealed class JsonArray : JsonValue<List<JsonValue>>, IEnumerable
{
    public JsonArray() : base(JsonToken.Array, new List<JsonValue>()) {}

    public JsonArray(IList<JsonValue> list) : base(JsonToken.Array, new List<JsonValue>()) 
    {
        foreach (var l in list)
            Value.Add(l);
    }

    public override JsonValue this[int idx] 
    { 
        get => Value[idx]; 
        set => Value[idx] = value; 
    }

    public override int Count => Value.Count;
    public override IEnumerable<JsonValue> Values => Value;

    public override JsonArray AsJsonArray => this;

    public override void Add(JsonValue value)
    {
        Value.Add(value);
    }

    public override void Remove(JsonValue value)
    {
        Value.Remove(value);
    }

    public override JsonValue[] ToArray()
    {
        return Value.ToArray();
    }

    public override List<JsonValue> ToList()
    {
        return Value;
    }

    public override bool Contains(JsonValue value)
    {
        return Value.Contains(value);
    }

    public IEnumerator GetEnumerator()
    {
        return Value.GetEnumerator();
    }

    public override string ToString()
    {
        return ToString(new JsonTextWriterOptions { Minimal = true });
    }

    public override string ToString(JsonTextWriterOptions options)
    {
    
        return JsonTextWriter.Write(this, options);
    }
}