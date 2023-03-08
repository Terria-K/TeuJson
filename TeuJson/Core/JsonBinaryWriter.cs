using System;
using System.Collections.Generic;
using System.IO;

namespace TeuJson;

public sealed partial class JsonBinaryWriter : JsonWriter, IDisposable
{
    private readonly Stack<long> positions = new Stack<long>();
    private readonly BinaryWriter writer;

    private JsonBinaryWriter(Stream fs) 
    {
        var writer = new BinaryWriter(fs);
        this.writer = writer;
    }

    public static void WriteToFile(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }

    public static void WriteToFile(Stream fs, JsonValue value) 
    {
        using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }

    public override void BeginArray()
    {
        writer.Write((byte)JsonHint.LBracket);
        positions.Push(writer.BaseStream.Position);
        writer.Write((uint)0);
    }

    public override void BeginObject()
    {
        writer.Write((byte)JsonHint.LParent);
        positions.Push(writer.BaseStream.Position);
        writer.Write((uint)0);
    }

    public void Dispose()
    {
        writer.Dispose();
    }

    public override void EndArray()
    {
        writer.Write((byte)JsonHint.RBracket);
        var current = writer.BaseStream.Position;
        var offset = positions.Pop();

        writer.BaseStream.Seek(offset, SeekOrigin.Begin);
        writer.Write((uint)(current - offset - 4));
        writer.BaseStream.Seek(current, SeekOrigin.Begin);
    }

    public override void EndObject()
    {
        writer.Write((byte)JsonHint.RParent);
        var current = writer.BaseStream.Position;
        var offset = positions.Pop();

        writer.BaseStream.Seek(offset, SeekOrigin.Begin);
        writer.Write((uint)(current - offset - 4));
        writer.BaseStream.Seek(current, SeekOrigin.Begin);
    }

    public override void WriteKey(string name)
    {
        writer.Write((byte)JsonHint.Key);
        writer.Write(name);
    }

    public override void WriteNull()
    {
        writer.Write((byte)JsonHint.Null);
    }

    public override void WriteValue(string? value) 
    {
        writer.Write((byte)JsonHint.String);
        writer.Write(value ?? "");
    }
}