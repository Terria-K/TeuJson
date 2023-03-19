using System;
using System.IO;
using System.Threading.Tasks;

namespace TeuJson;

/// <summary>
/// A list of functions that can be used for converting a Json object to C# object or vice versa.
/// </summary>
public static class JsonConvert 
{
    /// <summary>
    /// Parses a text containing Json to C# object from a file. 
    /// </summary>
    /// <param name="path">A path to Json file</param>
    /// <typeparam name="T">A type that implements IDeserialize</typeparam>
    /// <returns>A converted C# object from a Json object.</returns>
    public static T DeserializeFromFile<T>(string path) 
    where T : IDeserialize, new()
    {
        var converter = JsonTextReader.FromFile(path);
        return Deserialize<T>(converter.AsJsonObject);
    }

    /// <summary>
    /// Parses a text containing Json to C# object from a stream. 
    /// </summary>
    /// <param name="stream">A stream to use for getting the Json text</param>
    /// <typeparam name="T">A type that implements IDeserialize</typeparam>
    /// <returns>A converted C# object from a Json object.</returns>
    public static T DeserializeFromStream<T>(Stream stream) 
    where T : IDeserialize, new()
    {
        var converter = JsonTextReader.FromStream(stream);
        return Deserialize<T>(converter.AsJsonObject);
    }

    /// <summary>
    /// Parses a text into Json by stream.
    /// </summary>
    /// <param name="stream">A stream to use for getting the Json text</param>
    /// <returns>A parsed Json value.</returns>
    public static JsonValue DeserializeFromStream(Stream stream) 
    {
        var converter = JsonTextReader.FromStream(stream);
        return converter;
    }

    /// <summary>
    /// Parses a text into Json by file.
    /// </summary>
    /// <param name="path">A path to Json file</param>
    /// <returns>A parsed Json value.</returns>
    public static JsonValue DeserializeFromFile(string path) 
    {
        var converter = JsonTextReader.FromFile(path);
        return converter;
    }

    /// <summary>
    /// Parses a binary data containing Json to C# object from a file. 
    /// </summary>
    /// <param name="path">A path to Json binary</param>
    /// <typeparam name="T">A type that implements IDeserialize</typeparam>
    /// <returns>A converted C# object from a Json object.</returns>
    public static T DeserializeFromFileBinary<T>(string path) 
    where T : IDeserialize, new()
    {
        var converter = JsonBinaryReader.FromFile(path);
        return Deserialize<T>(converter.AsJsonObject);
    }

    /// <summary>
    /// Parses a binary data containing Json to C# object from a stream. 
    /// </summary>
    /// <param name="stream">A stream to use to handle bytes of data</param>
    /// <typeparam name="T">A type that implements IDeserialize</typeparam>
    /// <returns>A converted C# object from a Json object.</returns>
    public static T DeserializeFromStreamBinary<T>(Stream fs) 
    where T : IDeserialize, new()
    {
        var converter = JsonBinaryReader.FromStream(fs);
        return Deserialize<T>(converter.AsJsonObject);
    }

    /// <summary>
    /// Deserialize a Json value to C# object.
    /// </summary>
    /// <param name="value">A Json value to be deserialized</param>
    /// <typeparam name="T">A type that implements IDeserialize</typeparam>
    /// <returns>A converted C# object from a Json object.</returns>
    public static T Deserialize<T>(JsonValue value)
    where T : IDeserialize, new() 
    {
        var obj = new T();
        obj.Deserialize(value.AsJsonObject);
        return obj;
    }

    /// <summary>
    /// Serialize a C# object to Json value. 
    /// </summary>
    /// <param name="serializable">A type that implements ISerialize to be serialized</param>
    /// <returns>A serialized Json value</returns>
    public static JsonValue Serialize(ISerialize serializable) 
    {
        if (serializable == null)
            return JsonNull.NullReference;
        return serializable.Serialize();
    }


    /// <summary>
    /// Serialize a C# object to Json value and directly write it to a file. 
    /// </summary>
    /// <param name="serializable">A type that implements ISerialize to be serialized</param>
    /// <param name="path">A path to create or write a file</param>
    /// <param name="pretty">A boolean value that format if it can be pretty or minimal</param>
    public static void SerializeToFile(ISerialize serializable, string path, bool pretty = true) 
    {
        JsonTextWriter.WriteToFile(path, serializable.Serialize(), new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }

    /// <summary>
    /// Serialize a C# object to Json value and directly write it to a stream. 
    /// </summary>
    /// <param name="serializable">A type that implements ISerialize to be serialized</param>
    /// <param name="path">A stream to write</param>
    /// <param name="pretty">A boolean value that format if it can be pretty or minimal</param>
    public static void SerializeToStream(JsonValue value, Stream fs, bool pretty = true) 
    {
        JsonTextWriter.WriteToStream(fs, value, new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }

    /// <summary>
    /// Serialize a C# object to Json value and directly write it to a file asynchronously. 
    /// </summary>
    /// <param name="serializable">A type that implements ISerialize to be serialized</param>
    /// <param name="path">A path to create or write a file</param>
    /// <param name="pretty">A boolean value that format if it can be pretty or minimal</param>
    public static async Task SerializeToFileAsync(ISerialize serializable, string path, bool pretty = true) 
    {
        await JsonTextWriter.WriteToFileAsync(path, serializable.Serialize(), new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }

    /// <summary>
    /// Serialize a C# object to Json value and directly write it to a stream asynchronously. 
    /// </summary>
    /// <param name="serializable">A type that implements ISerialize to be serialized</param>
    /// <param name="path">A stream to write</param>
    /// <param name="pretty">A boolean value that format if it can be pretty or minimal</param>
    public static async Task SerializeToStreamAsync(JsonValue value, Stream fs, bool pretty = true) 
    {
        await JsonTextWriter.WriteToStreamAsync(fs, value, new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }
}