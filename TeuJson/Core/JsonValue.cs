using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TeuJson;

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
    public abstract short AsInt16 { get; }
    public abstract int AsInt32 { get; }
    public abstract long AsInt64 { get; }
    public abstract sbyte AsSByte { get; }
    public abstract ushort AsUInt16 { get; }
    public abstract uint AsUInt32 { get; }
    public abstract ulong AsUInt64 { get; }
    public abstract float AsSingle { get; }
    public abstract double AsDouble { get; }
    public abstract decimal AsDecimal { get; }
    public abstract bool AsBoolean { get; }
    public abstract string AsString { get; }
    public abstract JsonArray AsJsonArray { get; }
    public abstract JsonObject AsJsonObject { get; }
    public abstract object? AsObject { get; }

#if !NETFRAMEWORK
    public abstract nint AsIntPtr { get; }
    public abstract nuint AsUIntPtr { get; }
#endif


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
    public abstract JsonValue[] ToArray();
    public abstract List<JsonValue> ToList();
    public abstract Dictionary<string, JsonValue> ToDictionary();
    public static implicit operator JsonValue(bool value) => new JsonValue<bool>(JsonToken.Boolean, value);
    public static implicit operator JsonValue(decimal value) => new JsonValue<decimal>(JsonToken.Number, value);
    public static implicit operator JsonValue(float value) => new JsonValue<float>(JsonToken.Number, value);
    public static implicit operator JsonValue(double value) => new JsonValue<double>(JsonToken.Number, value);
    public static implicit operator JsonValue(byte value) => new JsonValue<byte>(JsonToken.Number, value);
    public static implicit operator JsonValue(sbyte value) => new JsonValue<sbyte>(JsonToken.Number, value);
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
#if !NETFRAMEWORK
    public static implicit operator JsonValue(nint value) => new JsonValue<nint>(JsonToken.Number, value);
    public static implicit operator JsonValue(nuint value) => new JsonValue<nuint>(JsonToken.Number, value);
#endif

    public static implicit operator bool(JsonValue value) => value.AsBoolean;
    public static implicit operator float(JsonValue value) => value.AsSingle;
    public static implicit operator double(JsonValue value) => value.AsDouble;
    public static implicit operator byte(JsonValue value) => value.AsByte;
    public static implicit operator sbyte(JsonValue value) => value.AsSByte;
    public static implicit operator char(JsonValue value) => value.AsChar;
    public static implicit operator short(JsonValue value) => value.AsInt16;
    public static implicit operator ushort(JsonValue value) => value.AsUInt16;
    public static implicit operator int(JsonValue value) => value.AsInt32;
    public static implicit operator uint(JsonValue value) => value.AsUInt32;
    public static implicit operator long(JsonValue value) => value.AsInt64;
    public static implicit operator ulong(JsonValue value) => value.AsUInt64;
    public static implicit operator string(JsonValue value) => value.AsString;
#if !NETFRAMEWORK
    public static implicit operator nint(JsonValue value) => value.AsIntPtr;
    public static implicit operator nuint(JsonValue value) => value.AsUIntPtr;
#endif
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
        set => throw new InvalidOperationException(); 
    }
    public override JsonValue this[int idx] 
    { 
        get => JsonNull.NullReference;
        set => throw new InvalidOperationException(); 
    }

    public override char AsChar 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is char value)
                    return value;
                return Convert.ToChar(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && char.TryParse(str, out char result))
                return result;
            return (char)0;
        }
    }

    public override byte AsByte 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is byte value)
                    return value;
                return Convert.ToByte(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && byte.TryParse(str, out byte result))
                return result;
            return 0;
        }
    }

    public override short AsInt16 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is short value)
                    return value;
                return Convert.ToInt16(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && short.TryParse(str, out short result))
                return result;
            return 0;
        }
    }

    public override int AsInt32     
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is int value)
                    return value;
                return Convert.ToInt32(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && int.TryParse(str, out int result))
                return result;
            return 0;
        }
    }

#if !NETFRAMEWORK
    public override nint AsIntPtr 
    {
        get 
        {
            if (IsNumber && Value is nint value) 
                return value;
            
            if (IsString && Value is string str && nint.TryParse(str, out nint result))
                return result;
            return 0;
        }
    }
#endif

    public override long AsInt64
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is long value)
                    return value;
                return Convert.ToInt64(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && long.TryParse(str, out long result))
                return result;
            return 0;
        }
    }

    public override sbyte AsSByte    
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is sbyte value)
                    return value;
                return Convert.ToSByte(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && sbyte.TryParse(str, out sbyte result))
                return result;
            return 0;
        }
    }

    public override ushort AsUInt16 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is ushort value)
                    return value;
                return Convert.ToUInt16(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && ushort.TryParse(str, out ushort result))
                return result;
            return 0;
        }
    }

    public override uint AsUInt32 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is uint value)
                    return value;
                return Convert.ToUInt32(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && uint.TryParse(str, out uint result))
                return result;
            return 0;
        }
    }

    public override ulong AsUInt64 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is ulong value)
                    return value;
                return Convert.ToUInt64(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && ulong.TryParse(str, out ulong result))
                return result;
            return 0;
        }
    }

#if !NETFRAMEWORK
    public override nuint AsUIntPtr 
    {
        get 
        {
            if (IsNumber && Value is nuint value)  
                return value;
            if (IsString && Value is string str && nuint.TryParse(str, out nuint result))
                return result;
            return 0; 
        }
    }
#endif

    public override float AsSingle 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is float value)
                    return value;
                return Convert.ToSingle(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && float.TryParse(str, out float result))
                return result;
            return 0;
        }
    }

    public override double AsDouble 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is double value)
                    return value;
                return Convert.ToDouble(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && double.TryParse(str, out double result))
                return result;
            return 0;
        }
    }

    public override decimal AsDecimal 
    {
        get 
        {
            if (IsNumber) 
            {
                if (Value is decimal value)
                    return value;
                return Convert.ToDecimal(Value, NumberFormatInfo.InvariantInfo);
            }
            
            if (IsString && Value is string str && decimal.TryParse(str, out decimal result))
                return result;
            return 0;
        }
    }

    public override bool AsBoolean => Value is bool value && value;


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

    public override object? AsObject
    {
        get
        {
            return Value;
        }
    }

    public override JsonArray AsJsonArray => throw new InvalidOperationException();

    public override JsonObject AsJsonObject => throw new InvalidOperationException();

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

    public override JsonValue[] ToArray()
    {
        throw new System.InvalidOperationException();
    }

    public override Dictionary<string, JsonValue> ToDictionary()
    {
        throw new System.InvalidOperationException();
    }

    public override List<JsonValue> ToList()
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