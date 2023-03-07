

namespace JsonT;

using System.Collections.Generic;

public static partial class JsonUtility 
{

    public static byte[]? ConvertToArrayByte(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new byte[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<byte>? ConvertToListByte(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<byte>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static byte[,]? ConvertToArrayByte2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new byte[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static short[]? ConvertToArrayInt16(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new short[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<short>? ConvertToListInt16(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<short>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static short[,]? ConvertToArrayInt162D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new short[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static int[]? ConvertToArrayInt32(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new int[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<int>? ConvertToListInt32(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<int>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static int[,]? ConvertToArrayInt322D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new int[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static long[]? ConvertToArrayInt64(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new long[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<long>? ConvertToListInt64(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<long>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static long[,]? ConvertToArrayInt642D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new long[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static sbyte[]? ConvertToArraySbyte(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new sbyte[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<sbyte>? ConvertToListSbyte(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<sbyte>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static sbyte[,]? ConvertToArraySbyte2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new sbyte[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static ushort[]? ConvertToArrayUInt16(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new ushort[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<ushort>? ConvertToListUInt16(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<ushort>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static ushort[,]? ConvertToArrayUInt162D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new ushort[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static uint[]? ConvertToArrayUInt32(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new uint[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<uint>? ConvertToListUInt32(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<uint>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static uint[,]? ConvertToArrayUInt322D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new uint[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static ulong[]? ConvertToArrayUInt64(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new ulong[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<ulong>? ConvertToListUInt64(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<ulong>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static ulong[,]? ConvertToArrayUInt642D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new ulong[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static float[]? ConvertToArraySingle(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new float[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<float>? ConvertToListSingle(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<float>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static float[,]? ConvertToArraySingle2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new float[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static double[]? ConvertToArrayDouble(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new double[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<double>? ConvertToListDouble(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<double>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static double[,]? ConvertToArrayDouble2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new double[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static string[]? ConvertToArrayString(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new string[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<string>? ConvertToListString(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<string>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static string[,]? ConvertToArrayString2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new string[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    public static bool[]? ConvertToArrayBoolean(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var valueArray = new bool[arrayCount];
        for (int i = 0; i < arrayCount; i++) 
        {
            valueArray[i] = array[i];
        }
        return valueArray;
    }

    public static List<bool>? ConvertToListBoolean(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var array = value.AsJsonArray;
        var arrayCount = array.Count;
        var list = new List<bool>(arrayCount);
        for (int i = 0; i < arrayCount; i++) 
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static bool[,]? ConvertToArrayBoolean2D(this JsonValue value) 
    {
        if (value.IsNull)
            return null;
        var arrayX = value.AsJsonArray;
        var arrayY = arrayX[0].AsJsonArray;
        var array2D = new bool[arrayX.Count, arrayY.Count];
        for (int i = 0; i < arrayX.Count; i++) 
        {
            for (int j = 0; j < arrayY.Count; j++) 
            {
                array2D[i, j] = arrayX[i].AsJsonArray[j];
            }
        }
        return array2D;
    }
    
    public static decimal[]? ConvertToArrayDecimal(this JsonValue value) 
    {
        if (value.IsNull)
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
        if (value.IsNull)
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
        if (value.IsNull)
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
    public static char[]? ConvertToArrayChar(this JsonValue value) 
    {
        if (value.IsNull)
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
        if (value.IsNull)
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
        if (value.IsNull)
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
    }