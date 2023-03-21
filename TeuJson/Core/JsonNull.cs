using System.Collections.Generic;
using System.Linq;

namespace TeuJson;

public sealed class JsonNull : JsonValue
{
    internal static JsonNull NullReference = new();
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

    public override short AsInt16 => 0;

    public override int AsInt32 => 0;

    public override long AsInt64 => 0;

    public override sbyte AsSByte => 0;

    public override ushort AsUInt16 => 0;

    public override uint AsUInt32 => 0;

    public override ulong AsUInt64 => 0;

    public override float AsSingle => 0;

    public override double AsDouble => 0;

    public override decimal AsDecimal => 0;

    public override bool AsBoolean => false;

    public override string AsString => string.Empty;
#if !NETFRAMEWORK
    public override nint AsIntPtr => 0;

    public override nuint AsUIntPtr => 0;

    public override object? AsObject => null;
#endif

    public override int Count => 0;

    public override IEnumerable<string> Keys => Enumerable.Empty<string>();

    public override IEnumerable<JsonValue> Values => Enumerable.Empty<JsonValue>();

    public override IEnumerable<KeyValuePair<string, JsonValue>> Pairs => Enumerable.Empty<KeyValuePair<string, JsonValue>>();

    public override JsonArray AsJsonArray => new();

    public override JsonObject AsJsonObject => new();


    public override void Add(JsonValue value)
    {
        throw new System.NotImplementedException();
    }

    public override bool Contains(JsonValue value)
    {
        return false;
    }

    public override bool Contains(string value)
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

    public override string ToString(JsonTextWriterOptions options)
    {
        return ToString();
    }

    public override string ToString()
    {
        return "null";
    }
}