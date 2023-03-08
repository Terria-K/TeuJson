using System.IO;

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

    // public static T DeserializeFromFileBinary<T>(string path) 
    // where T : IJsonTDeserializable, new()
    // {
    //     var converter = JsonBinaryReader.Parse(path);
    //     return Deserialize<T>(converter.AsJsonObject);
    // }

    public static T Deserialize<T>(JsonObject jsObj)
    where T : ITeuJsonDeserializable, new() 
    {
        var obj = new T();
        obj.Deserialize(jsObj);
        return obj;
    }

    public static T Deserialize<T>(JsonValue jsObj)
    where T : ITeuJsonDeserializable, new() 
    {
        var obj = new T();
        obj.Deserialize(jsObj.AsJsonObject);
        return obj;
    }

    // public static string Serialize(IJsonSerializable serializable, bool pretty = false) 
    // {
    //     return serializable.Serialize().ToString(pretty);
    // }
}