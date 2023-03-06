using System.Collections.Generic;
using System.Linq;

namespace JsonT;

public abstract class JsonValue 
{
    public readonly JsonToken Token;

    public bool IsNull => Token == JsonToken.Null;
    public bool IsNumber => Token == JsonToken.Number;
    public bool IsBoolean => Token == JsonToken.Boolean;
    public bool IsString => Token == JsonToken.String;
    public bool IsObject => Token == JsonToken.Object;
    public bool IsArray => Token == JsonToken.Array;

    public abstract char AsChar { get; }
    public abstract byte AsByte { get; }
    public abstract short AsShort { get; }
    public abstract int AsInt { get; }
    public abstract long AsLong { get; }
    public abstract sbyte AsSbyte { get; }
    public abstract ushort AsUShort { get; }
    public abstract uint AsUInt { get; }
    public abstract ulong AsULong { get; }
    public abstract float AsFloat { get; }
    public abstract double AsDouble { get; }
    public abstract decimal AsDecimal { get; }
    public abstract bool AsBool { get; }
    public abstract string AsString { get; }


    public abstract JsonValue this[string key] { get; set; }
    public abstract JsonValue this[int idx] { get; set; }

    public JsonValue(JsonToken token) 
    {
        Token = token;
    }

    public abstract int Count { get; }
    public abstract IEnumerable<string> Keys { get; }
    public abstract IEnumerable<JsonValue> Values { get; }
    public abstract IEnumerable<KeyValuePair<string, JsonValue>> Pairs { get; }

    public abstract void Add(JsonValue value);
    public abstract void Remove(JsonValue value);
    public abstract bool Contains(JsonValue value);
    public static implicit operator JsonValue(bool value) => new JsonValue<bool>(JsonToken.Boolean, value);
    public static implicit operator JsonValue(decimal value) => new JsonValue<decimal>(JsonToken.Number, value);
    public static implicit operator JsonValue(float value) => new JsonValue<float>(JsonToken.Number, value);
    public static implicit operator JsonValue(double value) => new JsonValue<double>(JsonToken.Number, value);
    public static implicit operator JsonValue(byte value) => new JsonValue<byte>(JsonToken.Number, value);
    public static implicit operator JsonValue(char value) => new JsonValue<char>(JsonToken.Number, value);
    public static implicit operator JsonValue(short value) => new JsonValue<short>(JsonToken.Number, value);
    public static implicit operator JsonValue(ushort value) => new JsonValue<ushort>(JsonToken.Number, value);
    public static implicit operator JsonValue(int value) => new JsonValue<int>(JsonToken.Number, value);
    public static implicit operator JsonValue(uint value) => new JsonValue<uint>(JsonToken.Number, value);
    public static implicit operator JsonValue(long value) => new JsonValue<long>(JsonToken.Number, value);
    public static implicit operator JsonValue(ulong value) => new JsonValue<ulong>(JsonToken.Number, value);
    public static implicit operator JsonValue(string? value) => new JsonValue<string>(JsonToken.String, value ?? "");
    public static implicit operator JsonValue(List<JsonValue> value) => new JsonArray(value);
    public static implicit operator JsonValue(JsonValue[] value) => new JsonArray(value);
}

public class JsonValue<T> : JsonValue
{
    public readonly T Value;
    public JsonValue(JsonToken token, T value) : base(token)
    {
        Value = value;
    }

    public override JsonValue this[string key] 
    { 
        get => JsonNull.NullReference;
        set => throw new System.InvalidOperationException(); 
    }
    public override JsonValue this[int idx] 
    { 
        get => JsonNull.NullReference;
        set => throw new System.InvalidOperationException(); 
    }

    public override char AsChar 
    {
        get 
        {
            if (IsNumber && Value is char value) 
                return value;
            
            if (IsString && Value is string str && char.TryParse(str, out char result))
                return result;
            return (char)0;
        }
    }

    public override byte AsByte 
    {
        get 
        {
            if (IsNumber && Value is byte value) 
                return value;
            
            if (IsString && Value is string str && byte.TryParse(str, out byte result))
                return result;
            return 0;
        }
    }

    public override short AsShort 
    {
        get 
        {
            if (IsNumber && Value is short value) 
                return value;
            
            if (IsString && Value is string str && short.TryParse(str, out short result))
                return result;
            return 0;
        }
    }

    public override int AsInt     
    {
        get 
        {
            if (IsNumber && Value is int value) 
                return value;
            
            if (IsString && Value is string str && int.TryParse(str, out int result))
                return result;
            return 0;
        }
    }

    public override long AsLong
    {
        get 
        {
            if (IsNumber && Value is long value) 
                return value;
            
            if (IsString && Value is string str && long.TryParse(str, out long result))
                return result;
            return 0;
        }
    }

    public override sbyte AsSbyte    
    {
        get 
        {
            if (IsNumber && Value is sbyte value) 
                return value;
            
            if (IsString && Value is string str && sbyte.TryParse(str, out sbyte result))
                return result;
            return 0;
        }
    }

    public override ushort AsUShort 
    {
        get 
        {
            if (IsNumber && Value is ushort value) 
                return value;
            
            if (IsString && Value is string str && ushort.TryParse(str, out ushort result))
                return result;
            return 0;
        }
    }

    public override uint AsUInt 
    {
        get 
        {
            if (IsNumber && Value is uint value) 
                return value;
            
            if (IsString && Value is string str && uint.TryParse(str, out uint result))
                return result;
            return 0;
        }
    }

    public override ulong AsULong 
    {
        get 
        {
            if (IsNumber && Value is ulong value) 
                return value;
            
            if (IsString && Value is string str && ulong.TryParse(str, out ulong result))
                return result;
            return 0;
        }
    }

    public override float AsFloat 
    {
        get 
        {
            if (IsNumber && Value is float value) 
                return value;
            
            if (IsString && Value is string str && float.TryParse(str, out float result))
                return result;
            return 0;
        }
    }

    public override double AsDouble 
    {
        get 
        {
            if (IsNumber && Value is double value) 
                return value;
            
            if (IsString && Value is string str && double.TryParse(str, out double result))
                return result;
            return 0;
        }
    }

    public override decimal AsDecimal 
    {
        get 
        {
            if (IsNumber && Value is decimal value) 
                return value;
            
            if (IsString && Value is string str && decimal.TryParse(str, out decimal result))
                return result;
            return 0;
        }
    }

    public override bool AsBool => (Value is bool value ? value : false);

    public override string AsString 
    {
        get 
        {
            if (IsString && Value is string str) 
                return str;
            if (Value != null)
                return Value.ToString() ?? "";
            return "";
        }
    }

    public override int Count => 0;

    public override IEnumerable<string> Keys => Enumerable.Empty<string>();

    public override IEnumerable<JsonValue> Values => Enumerable.Empty<JsonValue>();

    public override IEnumerable<KeyValuePair<string, JsonValue>> Pairs => Enumerable.Empty<KeyValuePair<string, JsonValue>>();

    public override void Add(JsonValue value)
    {
        throw new System.InvalidOperationException();
    }

    public override bool Contains(JsonValue value)
    {
        return false;
    }

    public override void Remove(JsonValue value)
    {
        throw new System.InvalidOperationException();
    }

    public override string ToString()
    {
        if (Value == null)
            return "null";
        return Value.ToString() ?? "";
    }
}