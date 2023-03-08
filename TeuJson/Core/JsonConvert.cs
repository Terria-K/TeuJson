using System.IO;
using System.Text;

namespace TeuJson;

public class JsonConvert 
{
    public static T DeserializeFromFile<T>(string path) 
    where T : ITeuJsonDeserializable, new()
    {
        var converter = JsonTextReader.FromFile(path);
        return Deserialize<T>(converter.AsJsonObject);
    }

    public static T DeserializeFromStream<T>(Stream stream) 
    where T : ITeuJsonDeserializable, new()
    {
        var converter = JsonTextReader.FromStream(stream);
        return Deserialize<T>(converter.AsJsonObject);
    }

    public static JsonValue DeserializeFromStream(Stream stream) 
    {
        var converter = JsonTextReader.FromStream(stream);
        return converter;
    }
    public static JsonValue DeserializeFromFile(string path) 
    {
        var converter = JsonTextReader.FromFile(path);
        return converter;
    }

    public static T DeserializeFromFileBinary<T>(string path) 
    where T : ITeuJsonDeserializable, new()
    {
        var converter = JsonBinaryReader.FromFile(path);
        return Deserialize<T>(converter.AsJsonObject);
    }

    public static T DeserializeFromStreamBinary<T>(Stream fs) 
    where T : ITeuJsonDeserializable, new()
    {
        var converter = JsonBinaryReader.FromStream(fs);
        return Deserialize<T>(converter.AsJsonObject);
    }

    public static T Deserialize<T>(JsonValue jsObj)
    where T : ITeuJsonDeserializable, new() 
    {
        var obj = new T();
        obj.Deserialize(jsObj.AsJsonObject);
        return obj;
    }

    public static JsonValue Serialize(ITeuJsonSerializable serializable) 
    {
        return serializable.Serialize();
    }

    public static void SerializeToFile(ITeuJsonSerializable serializable, string path, bool pretty = true) 
    {
        JsonTextWriter.WriteToFile(path, serializable.Serialize(), new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }

    public static void SerializeToStream(JsonValue value, Stream fs, bool pretty = true) 
    {
        JsonTextWriter.WriteToStream(fs, value, new JsonTextWriterOptions 
        {
            Minimal = !pretty
        });
    }
}