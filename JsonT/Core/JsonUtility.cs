using System.Collections.Generic;

namespace JsonT;

public static partial class JsonUtility 
{
    public static T? Convert<T>(this JsonValue value) 
    where T : IJsonTDeserializable, new()
    {
        if (value.IsNull) 
            return default;
        var obj = new T();
        obj.Deserialize(value.AsJsonObject);
        return obj;
    }

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

    public static Dictionary<string, T>? ToDictionary<T>(this JsonValue value)
    where T : IJsonTDeserializable, new()
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


    public static T[]? ConvertToArray<T>(this JsonValue value) 
    where T : IJsonTDeserializable, new()
    {
        if (value.IsNull)
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

    public static T[,]? ConvertToArray2D<T>(this JsonValue value) 
    where T : IJsonTDeserializable, new()
    {
        if (value.IsNull)
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

    public static int[,]? ConvertToArrayInt2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var intArray2D = new int[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                intArray2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return intArray2D;
    }

    public static List<T>? ConvertToList<T>(this JsonValue value) 
    where T : IJsonTDeserializable, new()
    {
        if (value.IsNull)
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

    public static JsonArray ConvertToJsonArray<T>(this List<T> array) 
    where T : IJsonTSerializable
    {
        var jsonArray = new JsonArray();
        foreach (T v in array) 
        {
            jsonArray.Add(v.Serialize());
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject<T>(this Dictionary<string, T> value)
    where T : IJsonTSerializable
    {
        var jsonObj = new JsonObject();
        foreach (var cObj in value) 
        {
            jsonObj[cObj.Key] = cObj.Value.Serialize();
        }
        return jsonObj;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, JsonValue> value)
    {
        var jsonObj = new JsonObject();
        foreach (var cObj in value) 
        {
            jsonObj[cObj.Key] = cObj.Value;
        }
        return jsonObj;
    }
}