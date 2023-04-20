namespace TeuJson;

public abstract class JsonWriter 
{
    public abstract void WriteKey(string name);
    public abstract void BeginObject();
    public abstract void EndObject();
    public abstract void BeginArray();
    public abstract void EndArray();

    public abstract void WriteNull();
    public abstract void WriteValue(byte value);
    public abstract void WriteValue(short value);
    public abstract void WriteValue(int value);
    public abstract void WriteValue(long value);
    public abstract void WriteValue(nint value);
    public abstract void WriteValue(sbyte value);
    public abstract void WriteValue(ushort value);
    public abstract void WriteValue(uint value);
    public abstract void WriteValue(ulong value);
    public abstract void WriteValue(nuint value);
    public abstract void WriteValue(float value);
    public abstract void WriteValue(double value);
    public abstract void WriteValue(decimal value);
    public abstract void WriteValue(bool value);
    public abstract void WriteValue(char value);
    public abstract void WriteValue(string value);


    public void WriteJson(JsonValue value) 
    {
        if (value == null) 
        {
            WriteNull();
            return;
        }
        switch (value.Token) 
        {
            case JsonToken.Object:
                BeginObject();
                foreach (var valueKey in value.Pairs) 
                {
                    WriteKey(valueKey.Key);
                    WriteJson(valueKey.Value);
                }
                EndObject();
                return;
            case JsonToken.Array:
                BeginArray();
                foreach (var val in value.Values) 
                {
                    WriteJson(val);
                }
                EndArray();
                return;
            case JsonToken.Boolean:
                WriteValue(value.AsBoolean);
                return;
            case JsonToken.String:
                WriteValue(value.AsString);
                return;
            case JsonToken.Number:
                if (value is JsonValue<decimal> Decimal)
                {
                    WriteValue(Decimal.AsDecimal);
                    return;
                }
                if (value is JsonValue<float> Float)
                {
                    WriteValue(Float.AsSingle);
                    return;
                }
                if (value is JsonValue<double> Double)
                {
                    WriteValue(Double.AsDouble);
                    return;
                }
                if (value is JsonValue<byte> Byte)
                {
                    WriteValue(Byte.AsByte);
                    return;
                }
                if (value is JsonValue<char> Char)
                {
                    WriteValue(Char.AsChar);
                    return;
                }
                if (value is JsonValue<short> Short)
                {
                    WriteValue(Short.AsInt16);
                    return;
                }
                if (value is JsonValue<ushort> UShort)
                {
                    WriteValue(UShort.AsUInt16);
                    return;
                }
                if (value is JsonValue<int> Int)
                {
                    WriteValue(Int.AsInt32);
                    return;
                }
                if (value is JsonValue<uint> UInt)
                {
                    WriteValue(UInt.AsUInt32);
                    return;
                }
#if NET6_0_OR_GREATER
                if (value is JsonValue<nint> NInt)
                {
                    WriteValue(NInt.AsIntPtr);
                    return;
                }
                if (value is JsonValue<nuint> NUInt)
                {
                    WriteValue(NUInt.AsUIntPtr);
                    return;
                }
#endif
                if (value is JsonValue<long> Long)
                {
                    WriteValue(Long.AsInt64);
                    return;
                }
                if (value is JsonValue<ulong> ULong)
                {
                    WriteValue(ULong.AsUInt64);
                    return;
                }
                break;
        }
        WriteNull();
    }
}