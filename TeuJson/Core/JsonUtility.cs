using System.Collections.Generic;

namespace TeuJson;

/// <summary>
/// A utility class used for converting into a different types, usually used by generator.
/// </summary>
public static partial class JsonUtility 
{
    /// <summary>
    /// Convert a Json value into C# Object.
    /// </summary>
    /// <param name="value">A Json value to be converted</param>
    /// <typeparam name="T">A type implements IDeserialize</typeparam>
    /// <returns>A parsed C# object</returns>
    public static T? Convert<T>(this JsonValue value) 
    where T : IDeserialize, new()
    {
        if (value.IsNull) 
            return default;
        var obj = new T();
        obj.Deserialize(value.AsJsonObject);
        return obj;
    }

    /// <summary>
    /// Extract Json values from a json object to be used as a C# dictionary. 
    /// </summary>
    /// <param name="value">A Json value to be converted</param>
    /// <returns>A C# dictionary attached with json values</returns>
    public static Dictionary<string, JsonValue>? ToDictionary(this JsonValue value)
    {
        if (value.IsNull) 
            return null;
        var dict = new Dictionary<string, JsonValue>();
        var jsonObject = value.AsJsonObject;
        foreach (var jsObj in jsonObject.AsJsonObject.Pairs) 
        {
            dict.Add(jsObj.Key, jsObj.Value);
        }
        return dict;
    }

    /// <summary>
    /// Convert Json values and Json object into an actual types and C# dictionary. 
    /// </summary>
    /// <param name="value">A Json value to be converted</param>
    /// <typeparam name="T">A type implements IDeserialize</typeparam>
    /// <returns>A C# dictionary with C# objects</returns>
    public static Dictionary<string, T>? ToDictionary<T>(this JsonValue value)
    where T : IDeserialize, new()
    {
        if (value.IsNull) 
            return null;
        var dict = new Dictionary<string, T>();
        var jsonObject = value.AsJsonObject;
        foreach (var jsObj in jsonObject.AsJsonObject.Pairs) 
        {
            dict.Add(jsObj.Key, JsonConvert.Deserialize<T>(jsObj.Value));
        }
        return dict;
    }

    /// <summary>
    /// Extract Json values from a json object to be used as a C# dictionary. 
    /// </summary>
    /// <param name="value">A Json value to be converted</param>
    /// <returns>A C# dictionary attached with json values</returns>
    public static Dictionary<string, JsonValue>? AsDictionary(this JsonValue value)
    {
        if (value.IsNull) 
            return null;
        var dict = new Dictionary<string, JsonValue>();
        var jsonObject = value.AsJsonObject;
        foreach (var jsObj in jsonObject.AsJsonObject.Pairs) 
        {
            dict.Add(jsObj.Key, jsObj.Value);
        }
        return dict;
    }

    /// <summary>
    /// Convert a Json array into C# array with strict types. 
    /// </summary>
    /// <param name="value">A Json array</param>
    /// <typeparam name="T">A type implements IDeserialize</typeparam>
    /// <returns>A C# array</returns>
    public static T[]? ConvertToArray<T>(this JsonValue value) 
    where T : IDeserialize, new()
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count; 
        var objectArray = new T[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            objectArray[i] = JsonConvert.Deserialize<T>(array[i]);
        }
        return objectArray;
    }

    public static T[]? ConvertToEnumArrayInt<T>(this JsonValue value) 
    where T : struct, System.Enum
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count; 
        var objectArray = new T[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            if (System.Enum.TryParse<T>(array[i].AsString, out T num)) 
            {
                objectArray[i] = num;
                continue;
            }
            objectArray[i] = (T)System.Enum.ToObject(typeof(T), array[i].AsInt32);
        }
        return objectArray;
    }

    /// <summary>
    /// Convert a 2D Json array into C# multi-dimensional array with strict types. 
    /// </summary>
    /// <param name="value">A Json array</param>
    /// <typeparam name="T">A type implements IDeserialize</typeparam>
    /// <returns>A C# multi-dimensional array</returns>
    public static T[,]? ConvertToArray2D<T>(this JsonValue value) 
    where T : IDeserialize, new()
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var objArray2D = new T[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                objArray2D[i, j] = JsonConvert.Deserialize<T>(arrayX[i].AsJsonArray[j]);
            }
        }
        return objArray2D;
    }

    /// <summary>
    /// Convert a Json array into C# list with strict types. 
    /// </summary>
    /// <param name="value">A Json array</param>
    /// <typeparam name="T">A type implements IDeserialize</typeparam>
    /// <returns>A C# list</returns>
    public static List<T>? ConvertToList<T>(this JsonValue value) 
    where T : IDeserialize, new()
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count; 
        var objectArray = new List<T>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            objectArray.Add(JsonConvert.Deserialize<T>(array[i]));
        }
        return objectArray;
    }

    /// <summary>
    /// Convert a C# array into Json array.
    /// </summary>
    /// <param name="value">An array of objects</param>
    /// <typeparam name="T">A type implements ISerialize</typeparam>
    /// <returns>A Json Array</returns>
    public static JsonArray ConvertToJsonArray<T>(this T[]? array) 
    where T : ISerialize
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (T v in array) 
        {
            jsonArray.Add(v.Serialize());
        }
        return jsonArray;
    }


    /// <summary>
    /// Convert a C# list into Json array.
    /// </summary>
    /// <param name="value">A list of objects</param>
    /// <typeparam name="T">A type implements ISerialize</typeparam>
    /// <returns>A Json Array</returns>
    public static JsonArray ConvertToJsonArray<T>(this List<T>? array) 
    where T : ISerialize
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (T v in array) 
        {
            jsonArray.Add(v.Serialize());
        }
        return jsonArray;
    }

    /// <summary>
    /// Convert a C# dictionary into Json object.
    /// </summary>
    /// <param name="value">A C# dictionary</param>
    /// <typeparam name="T">A type implements ISerialize</typeparam>
    /// <returns>A Json object</returns>
    public static JsonObject ToJsonObject<T>(this Dictionary<string, T>? value)
    where T : ISerialize
    {
        if (value == null)
            return new JsonObject();
        var jsonObj = new JsonObject();
        foreach (var cObj in value) 
        {
            jsonObj[cObj.Key] = cObj.Value.Serialize();
        }
        return jsonObj;
    }

    /// <summary>
    /// Convert an extracted Json value from a dictionary into Json object.
    /// </summary>
    /// <param name="value">An extracted json value from a dictionary</param>
    /// <returns>A Json object</returns>
    public static JsonObject ToJsonObject(this Dictionary<string, JsonValue>? value)
    {
        if (value == null)
            return new JsonObject();
        var jsonObj = new JsonObject();
        foreach (var cObj in value) 
        {
            jsonObj[cObj.Key] = cObj.Value;
        }
        return jsonObj;
    }
}