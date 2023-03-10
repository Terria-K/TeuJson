using System.Collections.Generic;

namespace TeuJson;


public static partial class JsonUtility 
{
    public static byte[]? ConvertToArrayByte(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new byte[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsByte;
        }
        return valueArray;
    }

    public static List<byte>? ConvertToListByte(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<byte>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsByte);
        }
        return list;
    }

    public static byte[,]? ConvertToArrayByte2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new byte[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsByte;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<byte> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (byte v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this byte[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (byte v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this byte[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, byte> value)
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

    public static short[]? ConvertToArrayInt16(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new short[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsInt16;
        }
        return valueArray;
    }

    public static List<short>? ConvertToListInt16(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<short>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsInt16);
        }
        return list;
    }

    public static short[,]? ConvertToArrayInt162D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new short[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsInt16;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<short> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (short v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this short[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (short v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this short[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, short> value)
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

    public static int[]? ConvertToArrayInt32(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new int[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsInt32;
        }
        return valueArray;
    }

    public static List<int>? ConvertToListInt32(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<int>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsInt32);
        }
        return list;
    }

    public static int[,]? ConvertToArrayInt322D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new int[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsInt32;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<int> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (int v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this int[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (int v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this int[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, int> value)
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

    public static long[]? ConvertToArrayInt64(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new long[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsInt64;
        }
        return valueArray;
    }

    public static List<long>? ConvertToListInt64(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<long>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsInt64);
        }
        return list;
    }

    public static long[,]? ConvertToArrayInt642D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new long[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsInt64;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<long> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (long v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this long[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (long v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this long[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, long> value)
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

    public static sbyte[]? ConvertToArraySByte(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new sbyte[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsSByte;
        }
        return valueArray;
    }

    public static List<sbyte>? ConvertToListSByte(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<sbyte>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsSByte);
        }
        return list;
    }

    public static sbyte[,]? ConvertToArraySByte2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new sbyte[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsSByte;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<sbyte> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (sbyte v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this sbyte[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (sbyte v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this sbyte[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, sbyte> value)
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

    public static ushort[]? ConvertToArrayUInt16(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new ushort[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsUInt16;
        }
        return valueArray;
    }

    public static List<ushort>? ConvertToListUInt16(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<ushort>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsUInt16);
        }
        return list;
    }

    public static ushort[,]? ConvertToArrayUInt162D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new ushort[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsUInt16;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<ushort> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (ushort v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this ushort[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (ushort v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this ushort[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, ushort> value)
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

    public static uint[]? ConvertToArrayUInt32(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new uint[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsUInt32;
        }
        return valueArray;
    }

    public static List<uint>? ConvertToListUInt32(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<uint>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsUInt32);
        }
        return list;
    }

    public static uint[,]? ConvertToArrayUInt322D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new uint[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsUInt32;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<uint> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (uint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this uint[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (uint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this uint[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, uint> value)
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

    public static ulong[]? ConvertToArrayUInt64(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new ulong[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsUInt64;
        }
        return valueArray;
    }

    public static List<ulong>? ConvertToListUInt64(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<ulong>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsUInt64);
        }
        return list;
    }

    public static ulong[,]? ConvertToArrayUInt642D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new ulong[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsUInt64;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<ulong> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (ulong v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this ulong[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (ulong v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this ulong[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, ulong> value)
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

    public static float[]? ConvertToArraySingle(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new float[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsSingle;
        }
        return valueArray;
    }

    public static List<float>? ConvertToListSingle(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<float>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsSingle);
        }
        return list;
    }

    public static float[,]? ConvertToArraySingle2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new float[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsSingle;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<float> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (float v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this float[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (float v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this float[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, float> value)
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

    public static double[]? ConvertToArrayDouble(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new double[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsDouble;
        }
        return valueArray;
    }

    public static List<double>? ConvertToListDouble(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<double>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsDouble);
        }
        return list;
    }

    public static double[,]? ConvertToArrayDouble2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new double[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsDouble;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<double> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (double v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this double[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (double v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this double[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, double> value)
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

    public static string[]? ConvertToArrayString(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new string[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsString;
        }
        return valueArray;
    }

    public static List<string>? ConvertToListString(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<string>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsString);
        }
        return list;
    }

    public static string[,]? ConvertToArrayString2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new string[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsString;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<string> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (string v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this string[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (string v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this string[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, string> value)
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

    public static bool[]? ConvertToArrayBoolean(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new bool[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsBoolean;
        }
        return valueArray;
    }

    public static List<bool>? ConvertToListBoolean(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<bool>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsBoolean);
        }
        return list;
    }

    public static bool[,]? ConvertToArrayBoolean2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new bool[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsBoolean;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<bool> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (bool v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this bool[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (bool v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this bool[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, bool> value)
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

    public static decimal[]? ConvertToArrayDecimal(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new decimal[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsDecimal;
        }
        return valueArray;
    }

    public static List<decimal>? ConvertToListDecimal(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<decimal>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsDecimal);
        }
        return list;
    }

    public static decimal[,]? ConvertToArrayDecimal2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new decimal[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsDecimal;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<decimal> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (decimal v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this decimal[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (decimal v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this decimal[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, decimal> value)
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

    public static char[]? ConvertToArrayChar(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new char[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsChar;
        }
        return valueArray;
    }

    public static List<char>? ConvertToListChar(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<char>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsChar);
        }
        return list;
    }

    public static char[,]? ConvertToArrayChar2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new char[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsChar;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<char> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (char v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this char[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (char v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this char[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, char> value)
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

    #if !NETFRAMEWORK
        public static nint[]? ConvertToArrayIntPtr(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new nint[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsIntPtr;
        }
        return valueArray;
    }

    public static List<nint>? ConvertToListIntPtr(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<nint>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsIntPtr);
        }
        return list;
    }

    public static nint[,]? ConvertToArrayIntPtr2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new nint[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsIntPtr;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<nint> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (nint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this nint[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (nint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this nint[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, nint> value)
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
        public static nuint[]? ConvertToArrayUIntPtr(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new nuint[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i].AsUIntPtr;
        }
        return valueArray;
    }

    public static List<nuint>? ConvertToListUIntPtr(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<nuint>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i].AsUIntPtr);
        }
        return list;
    }

    public static nuint[,]? ConvertToArrayUIntPtr2D(this JsonValue value) 
    {
        if (value.IsNull || value.Count <= 0)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new nuint[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j].AsUIntPtr;
            }
        }
        return array2D;
    }

    public static JsonArray ConvertToJsonArray(this List<nuint> array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (nuint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray(this nuint[] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        foreach (nuint v in array) 
        {
            jsonArray.Add(v);
        }
        return jsonArray;
    }

    public static JsonArray ConvertToJsonArray2D(this nuint[,] array) 
    {
        if (array == null)
            return new JsonArray();
        var jsonArray = new JsonArray();
        for (int i = 0; i < array.GetLength(0); i++) 
        {
            var jsonArray2 = new JsonArray();
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                jsonArray2.Add(array[i, j]);
            }
            jsonArray.Add(jsonArray2);
        }
        return jsonArray;
    }

    public static JsonObject ToJsonObject(this Dictionary<string, nuint> value)
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
    #endif

}