using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TeuJson;

public struct JsonTextWriterOptions 
{
    public static JsonTextWriterOptions Default => new()
    {
        Minimal = false
    };
    public int ArrayDepth;
    public bool Minimal;
}

#if NETFRAMEWORK
public sealed class JsonTextWriter : JsonWriter, IDisposable
#else
public sealed class JsonTextWriter : JsonWriter, IDisposable, IAsyncDisposable
#endif
{
    private readonly TextWriter writer;
    private readonly bool minimal;
    private int indent;
    private Stack<byte> containerStack = new();

    private bool arrayBegin;
    private bool wasValue;
    private bool wasBracket;

    private JsonTextWriter(Stream fs) 
    {
        writer = new StreamWriter(fs);
    }

    private JsonTextWriter(Stream fs, JsonTextWriterOptions options) 
    {
        writer = new StreamWriter(fs);
        minimal = options.Minimal;
    }

    public static void WriteToFile(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    public static void WriteToFile(string path, JsonValue value, JsonTextWriterOptions options) 
    {
        using var fs = File.Create(path);
        using var textWriter = new JsonTextWriter(fs, options);
        textWriter.WriteJson(value);
    }

    public static void WriteToStream(Stream fs, JsonValue value) 
    {
        using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    public static void WriteToStream(Stream fs, JsonValue value, JsonTextWriterOptions options) 
    {
        using var textWriter = new JsonTextWriter(fs, options);
        textWriter.WriteJson(value);
    }

#if !NETFRAMEWORK
    public static async Task WriteToFileAsync(string path, JsonValue value) 
    {
        using var fs = File.Create(path);
        await using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    public static async Task WriteToFileAsync(string path, JsonValue value, JsonTextWriterOptions options) 
    {
        using var fs = File.Create(path);
        await using var textWriter = new JsonTextWriter(fs, options);
        textWriter.WriteJson(value);
    }

    public static async Task WriteToStreamAsync(Stream fs, JsonValue value) 
    {
        await using var textWriter = new JsonTextWriter(fs);
        textWriter.WriteJson(value);
    }

    public static async Task WriteToStreamAsync(Stream fs, JsonValue value, JsonTextWriterOptions options) 
    {
        await using var textWriter = new JsonTextWriter(fs, options);
        textWriter.WriteJson(value);
    }

    public async ValueTask DisposeAsync()
    {
        await writer.DisposeAsync();
    }
#endif

    private void Newline() 
    {
        if (minimal)
            return;
        writer.Write("\n");
        for (int i = 0; i < indent; i++)
            writer.Write("\t");
    }

    private void NextValue() 
    {
        if (wasValue)
            writer.Write(", ");
        if ((wasValue || wasBracket) && !arrayBegin) 
        {
            Newline();
        }
        else if (arrayBegin && containerStack.Peek() == 0) 
        {
            Newline();
        }
        

        wasValue = true;
        wasBracket = false;
    }

    private void NextKey() 
    {
        if (wasValue)
            writer.Write(",");
        Newline();
        wasValue = false;
        wasBracket = false;
    }
    
    private void NextBracket() 
    {
        if (wasValue)
            writer.Write(",");
        if (wasBracket)
            writer.Write("");
        wasBracket = true;
        wasValue = false;
    }

    public override void BeginArray()
    {
        containerStack.Push(0);
        NextBracket();
        writer.Write("[");
        indent++;
        arrayBegin = true;
    }

    public override void BeginObject()
    {
        containerStack.Push(1);
        NextBracket();
        writer.Write("{");
        indent++;
    }

    public void Dispose()
    {
        writer.Dispose();
    }

    public override void EndArray()
    {
        containerStack.Pop();
        indent--;
        if (wasBracket)
            writer.Write("");
        else   
            Newline();

        writer.Write("]");
        wasValue = true;
        wasBracket = false;
        arrayBegin = false;
    }

    public override void EndObject()
    {
        containerStack.Pop();
        indent--;
        if (wasBracket)
            writer.Write("");
        else   
            Newline();

        writer.Write("}");
        wasValue = true;
        wasBracket = false;
    }

    public override void WriteKey(string name)
    {
        NextKey();
        writer.Write($"\"{name}\"");
        writer.Write(": ");
    }

    public override void WriteNull()
    {
        NextValue();
        writer.Write("null");
    }

    public override void WriteValue(byte value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(short value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(int value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(long value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(sbyte value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(ushort value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(uint value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(ulong value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(float value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(double value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(decimal value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(bool value)
    {
        NextValue();
        writer.Write(value ? "true" : "false");
    }

    public override void WriteValue(char value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(string value)
    {
        NextValue();
        writer.Write("\"" + value + "\"");
    }

    public override void WriteValue(nint value)
    {
        NextValue();
        writer.Write(value);
    }

    public override void WriteValue(nuint value)
    {
        NextValue();
        writer.Write(value);
    }
}