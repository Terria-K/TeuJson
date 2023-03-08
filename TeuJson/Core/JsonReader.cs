using System;

namespace TeuJson;

public abstract class JsonReader 
{
    public JsonToken Token;
    public int Line = 1;
    public int Column;
    public object? Value;

    public JsonValue ReadObject() 
    {
        var value = new JsonObject();
        while (ReadInternal() && Token != JsonToken.RParent) 
        {
            if (Token == JsonToken.LParent) 
                continue;

            if (Token != JsonToken.Key)
                throw new Exception($"Expected a key, found: {Token} on Line {Line} Column {Column}");

            var key = Value as string;
            if (key != null) 
            {
                value[key] = ReadValue();
            }
        }
        return value;
    }

    private JsonArray ReadArray() 
    {
        var list = new JsonArray();
        while (ReadInternal() && Token != JsonToken.RBracket)
            list.Add(ReadValueFromToken());
        
        return list;
    }

    private JsonValue ReadValue() 
    {
        ReadInternal();
        var token = ReadValueFromToken();
        return token;
    }

    private JsonValue ReadValueFromToken() 
    {
        switch (Token) 
        {
            case JsonToken.LParent:
                return ReadObject();    
            case JsonToken.LBracket:
                return ReadArray();
        }
        
        return Value switch 
        {
            bool value => value,
            byte value => value,
            short value => value,
            int value => value,
            long value => value,
            sbyte value => value,
            ushort value => value,
            uint value => value,
            ulong value => value,
            float value => value,
            double value => value,
            decimal value => value,
            string value => value,
            _ => JsonNull.NullReference
        };
    }

    protected abstract bool ReadInternal();
}
