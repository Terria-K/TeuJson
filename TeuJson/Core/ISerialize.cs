namespace TeuJson;

public interface ISerialize 
{
    /// <summary>
    /// Serialize a C# object to convert it into Json object.
    /// </summary>
    /// <returns>A parsed Json object</returns>
    JsonObject Serialize();
}