using System;
using System.IO;
using System.Text;

namespace JsonT;

public sealed class JsonTextReader : JsonReader, IDisposable
{
    private readonly StreamReader reader;
    private readonly Stream stream;
    private readonly StringBuilder builder = new StringBuilder();

    public JsonTextReader(string path) 
    {
        stream = File.OpenRead(path);
        reader = new StreamReader(stream);
    }

    public JsonTextReader(Stream fs) 
    {
        this.stream = fs;
        reader = new StreamReader(fs);
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
            builder.Append(next);
            while (ReadChar(out next) && !("\r\n,").Contains(next)) 
            {
                builder.Append(next);
            }
            var str = builder.ToString();

            if (str[0] == 't') 
            {
                Token = JsonToken.Boolean;
                Value = true;
                return true;
            }

            if (str[0] == 'f') 
            {
                Token = JsonToken.Boolean;
                Value = false;
                return true;
            }

            if (str[0] >= '0' && str[0] <= '9') 
            {
                Token = JsonToken.Number;
                Value = int.Parse(str);
                return true;
            }
            throw new Exception("Value not found!");
        }
        return false;
    }

    private bool ReadChar(out char next) 
    {
        int read = reader.Read();
        next = (char)read;
        Position++;
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
        stream.Dispose();
    }
}