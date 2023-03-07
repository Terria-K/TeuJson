using System;
using System.IO;
using System.Text;

namespace JsonT;

public sealed class JsonTextReader : JsonReader, IDisposable
{
    private readonly TextReader reader;
    private readonly StringBuilder builder = new StringBuilder();

    private JsonTextReader(Stream fs) 
    {
        reader = new StreamReader(fs);
    }

    public static JsonValue FromFile(string path) 
    {
        using var reader = File.OpenRead(path);
        using var textReader = new JsonTextReader(reader);
        return textReader.ReadObject(); 
    }

    public static JsonValue FromStream(Stream fs) 
    {
        using var textReader = new JsonTextReader(fs);
        return textReader.ReadObject(); 
    }

    protected override bool ReadInternal()
    {
        while (ReadChar(out char next)) 
        {
            if (char.IsWhiteSpace(next) || next == ':' || next == ',') 
                continue;

            switch (next) 
            {
                case '{':
                    Token = JsonToken.LParent;
                    return true;
                case '}':
                    Token = JsonToken.RParent;
                    return true;
                case '[':
                    Token = JsonToken.LBracket;
                    return true;
                case ']':
                    Token = JsonToken.RBracket;
                    return true;
            }

            if (next == '"') 
            {
                // Check for key
                builder.Clear();
                while (ReadChar(out next) && next != '"') 
                {
                    builder.Append(next);
                }
                if (char.IsWhiteSpace(next)) 
                {
                    while (PeekChar(out next) && char.IsWhiteSpace(next))
                        SkipChar();
                }
                if (PeekChar(out char future) && future== ':') 
                {
                    Token = JsonToken.Key;
                    Value = builder.ToString();
                    return true;
                }
                // Read String Value
                // Might not be a good idea to separate it to other value
                Token = JsonToken.String;
                Value = builder.ToString();
                return true;
            }

            // Read Values

            // Get the text
            builder.Clear();
            var first = next;
            builder.Append(next);
            while (PeekChar(out next) && !("\r\n,}]").Contains(next)) 
            {
                builder.Append(next);
                SkipChar();
            }
            // Read the text
            var str = builder.ToString();

            if (str == "true") 
            {
                Token = JsonToken.Boolean;
                Value = true;
                return true;
            }

            if (str == "false") 
            {
                Token = JsonToken.Boolean;
                Value = false;
                return true;
            }

            if (ReadNumbers()) 
            {
                Token = JsonToken.Number;
                if (str.Contains(".")) 
                {
                    if (float.TryParse(str, out float fValue))
                    {
                        Value = fValue;
                        return true;
                    }

                    if (double.TryParse(str, out double dValue))
                    {
                        Value = dValue;
                        return true;
                    }
                }
                if (int.TryParse(str, out int iValue))
                {
                    Value = iValue;
                    return true;
                }

                if (long.TryParse(str, out long lValue))
                {
                    Value = lValue;
                    return true;
                }
                if (uint.TryParse(str, out uint uIValue)) 
                {
                    Value = uIValue;
                    return true;
                }
                if (ulong .TryParse(str, out ulong uLValue)) 
                {
                    Value = uLValue;
                    return true;
                }
            }
            bool ReadNumbers() 
            {
                return first == '0' 
                || char.IsDigit(first) 
                || first == '-'
                || first == '.'
                || first == 'e'
                || first == '+';
            }
            throw new Exception($"Value not found or invalid on Line {Line} Column {Column}!");
        }
        return false;


    }

    private bool ReadChar(out char next) 
    {
        int read = reader.Read();
        next = (char)read;
        if (next == '\n') 
        {
            Column = 0;
            Line++;
        }
        Column++;

        return read != -1;
    }

    private bool PeekChar(out char next) 
    {
        int read = reader.Peek();
        next = (char)read;
        return read != -1;
    }

    private bool SkipChar() 
    {
        return ReadChar(out _);
    }

    public void Dispose()
    {
        reader.Dispose();
    }
}