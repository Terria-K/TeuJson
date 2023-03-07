using System.Collections.Generic;
using System.Linq;

namespace JsonT;

public sealed class JsonNull : JsonValue
{
    internal static JsonNull NullReference = new JsonNull();
    public JsonNull() : base(JsonToken.Null)
    {
    }

    public override JsonValue this[string key] 
    { 
        get => NullReference;
        set => throw new System.InvalidOperationException(); 
    }
    public override JsonValue this[int idx] 
    { 
        get => NullReference;
        set => throw new System.InvalidOperationException(); 
    }

    public override char AsChar => (char)0;

    public override byte AsByte => 0;

    public override short AsShort => 0;

    public override int AsInt => 0;

    public override long AsLong => 0;

    public override sbyte AsSbyte => 0;

    public override ushort AsUShort => 0;

    public override uint AsUInt => 0;

    public override ulong AsULong => 0;

    public override float AsFloat => 0;

    public override double AsDouble => 0;

    public override decimal AsDecimal => 0;

    public override bool AsBool => false;

    public override string AsString => string.Empty;

    public override nint AsNullInt => 0;

    public override nuint AsUNullInt => 0;

    public override int Count => 0;

    public override IEnumerable<string> Keys => Enumerable.Empty<string>();

    public override IEnumerable<JsonValue> Values => Enumerable.Empty<JsonValue>();

    public override IEnumerable<KeyValuePair<string, JsonValue>> Pairs => Enumerable.Empty<KeyValuePair<string, JsonValue>>();

    public override JsonArray AsJsonArray => new JsonArray();

    public override JsonObject AsJsonObject => new JsonObject();

    public override void Add(JsonValue value)
    {
        throw new System.NotImplementedException();
    }

    public override bool Contains(JsonValue value)
    {
        return false;
    }

    public override void Remove(JsonValue value)
    {
        throw new System.NotImplementedException();
    }

    public override JsonValue[] ToArray()
    {
        throw new System.NullReferenceException();
    }

    public override Dictionary<string, JsonValue> ToDictionary()
    {
        throw new System.NullReferenceException();
    }

    public override List<JsonValue> ToList()
    {
        throw new System.NullReferenceException();
    }
}