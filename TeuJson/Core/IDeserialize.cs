namespace TeuJson;

public interface IDeserialize 
{
    /// <summary>
    /// Deserialize a Json object to convert into C# object.
    /// </summary>
    /// <param name="value">A Json object to used for deserialization</param>
    void Deserialize(JsonObject value);
}