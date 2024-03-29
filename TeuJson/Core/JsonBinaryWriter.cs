using System;
using System.Collections.Generic;
using System.IO;
#if NET5_0_OR_GREATER
using System.Threading.Tasks;
#endif

namespace TeuJson;

#if NET5_0_OR_GREATER
public sealed partial class JsonBinaryWriter : JsonWriter, IDisposable, IAsyncDisposable
#else
public sealed partial class JsonBinaryWriter : JsonWriter, IDisposable
#endif
{
    private readonly Stack<long> positions = new();
    private readonly BinaryWriter writer;

    private JsonBinaryWriter(Stream fs) 
    {
        var writer = new BinaryWriter(fs);
        this.writer = writer;
    }

    /// <summary>
    /// Write a Json binary from a Json value
    /// </summary>
    /// <param name="value">A Json value</param>
    /// <returns>A Json bytes representing a Json binary</returns>
    public static byte[] Write(JsonValue value) 
    {
        using var memStream = new MemoryStream();
        using var textWriter = new JsonBinaryWriter(memStream);
        textWriter.WriteJson(value);
        return memStream.ToArray();
    }

    /// <summary>
    /// Write a Json string to a file from a Json Value. 
    /// </summary>
    /// <param name="path">A path to write on</param>
    /// <param name="value">A Json value</param>
    public static void WriteToFile(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }

    /// <summary>
    /// Write a Json string to a stream from a Json Value. 
    /// </summary>
    /// <param name="fs">A stream to write on</param>
    /// <param name="value">A Json value</param>
    public static void WriteToFile(Stream fs, JsonValue value) 
    {
        using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }
#if NET5_0_OR_GREATER

    /// <summary>
    /// Write a Json string to a file from a Json Value asynchronously. 
    /// </summary>
    /// <param name="path">A path to write on</param>
    /// <param name="value">A Json value</param>
    public static async Task WriteToFileAsync(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        await using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }

    /// <summary>
    /// Write a Json string to a stream from a Json Value asynchronously. 
    /// </summary>
    /// <param name="fs">A stream to write on</param>
    /// <param name="value">A Json value</param>
    public static async Task WriteToFileAsync(Stream fs, JsonValue value) 
    {
        await using var textWriter = new JsonBinaryWriter(fs);
        textWriter.WriteJson(value);
    }

    public async ValueTask DisposeAsync()
    {
        await writer.DisposeAsync();
    }
#endif

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