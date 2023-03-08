using System;
using System.IO;

namespace TeuJson;

public sealed class JsonBinaryReader : JsonReader, IDisposable
{
    private readonly BinaryReader reader;

    private JsonBinaryReader(Stream fs) 
    {
        var reader = new BinaryReader(fs);
        this.reader = reader;
    }

    public static JsonValue FromFile(string path) 
    {
        using var reader = File.OpenRead(path);
        using var textReader = new JsonBinaryReader(reader);
        return textReader.ReadObject(); 
    }

    public static JsonValue FromStream(Stream fs) 
    {
        using var textReader = new JsonBinaryReader(fs);
        return textReader.ReadObject(); 
    }

    protected override bool ReadInternal()
    {
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
            return false;
        
        var token = (JsonHint)reader.ReadByte();

        switch (token)
        {
            case JsonHint.Null:
                Value = null;
                Token = JsonToken.Null;
                break;
            case JsonHint.LParent:
                reader.ReadUInt32();
                Value = null;
                Token = JsonToken.LParent;
                break;
            case JsonHint.RParent:
                Value = null;
                Token = JsonToken.RParent;
                break;
            case JsonHint.LBracket:
                reader.ReadUInt32();
                Value = null;
                Token = JsonToken.LBracket;
                break;
            case JsonHint.RBracket:
                Value = null;
                Token = JsonToken.RBracket;
                break;
            case JsonHint.Key:
                Value = reader.ReadString();
                Token = JsonToken.Key;
                break;
            case JsonHint.Byte:
                Value = reader.ReadByte();
                Token = JsonToken.Number;
                break;
            case JsonHint.Int16:
                Value = reader.ReadInt16();
                Token = JsonToken.Number;
                break;
            case JsonHint.Int32:
                Value = reader.ReadInt32();
                Token = JsonToken.Number;
                break;
            case JsonHint.Int64:
                Value = reader.ReadInt64();
                Token = JsonToken.Number;
                break;
            case JsonHint.SByte:
                Value = reader.ReadSByte();
                Token = JsonToken.Number;
                break;
            case JsonHint.UInt16:
                Value = reader.ReadUInt16();
                Token = JsonToken.Number;
                break;
            case JsonHint.UInt32:
                Value = reader.ReadUInt32();
                Token = JsonToken.Number;
                break;
            case JsonHint.UInt64:
                Value = reader.ReadUInt64();
                Token = JsonToken.Number;
                break;
            case JsonHint.Single:
                Value = reader.ReadSingle();
                Token = JsonToken.Number;
                break;
            case JsonHint.Double:
                Value = reader.ReadDouble();
                Token = JsonToken.Number;
                break;
            case JsonHint.Decimal:
                Value = reader.ReadDecimal();
                Token = JsonToken.Number;
                break;
            case JsonHint.Char:
                Value = reader.ReadChar();
                Token = JsonToken.Number;
                break;
            case JsonHint.String:
                Value = reader.ReadString();
                Token = JsonToken.String;
                break;
            case JsonHint.Boolean:
                Value = reader.ReadBoolean();
                Token = JsonToken.Boolean;
                break;
        }
        return true;
    }

    public void Dispose()
    {
        reader.Dispose();
    }
}