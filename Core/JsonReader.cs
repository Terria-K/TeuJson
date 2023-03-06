using System;
using System.Collections.Generic;

namespace JsonT;

public abstract class JsonReader 
{
    // will have json value soon
    public Dictionary<string, JsonToken> Tokenized = new Dictionary<string, JsonToken>();
    public JsonToken Token;
    public int Position;
    public object? Value;

    public JsonToken ReadObject() 
    {
        while (ReadInternal() && Token != JsonToken.RParent) 
        {
            if (Token == JsonToken.LParent) 
                continue;

            if (Token != JsonToken.Key)
                throw new Exception($"Expected a key, found: {Token}");

            var key = Value as string;
            if (key != null) 
            {
                Tokenized[key] = ReadValue();
                Console.WriteLine($"Key: {key}, Value: {Tokenized[key]}");
            }
        }
        return Token;
    }

    public JsonToken ReadArray() 
    {
        var list = new List<JsonToken>();
        while (ReadInternal() && Token != JsonToken.RBracket)
            list.Add(ReadValueFromToken());
        foreach (var l in list) 
        {
            Console.WriteLine("Array: " + l);
        }
        return JsonToken.RBracket;
    }

    public JsonToken ReadValue() 
    {
        ReadInternal();
        var token = ReadValueFromToken();
        Console.WriteLine(Value);
        return token;
    }

    private JsonToken ReadValueFromToken() 
    {
        switch (Token) 
        {
            case JsonToken.LParent:
                return ReadObject();    
            case JsonToken.LBracket:
                return ReadArray();
        }
        // for the time being
        if (Value is bool b)
            return JsonToken.Boolean;
        
        return Value switch {
            int => JsonToken.Number,
            string => JsonToken.String,
            _ => JsonToken.Null
        };
    }

    protected abstract bool ReadInternal();
}
