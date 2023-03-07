using System;
namespace JsonT.Generator;

partial class JsonTGenerator 
{
    private static string ArrayCheck(string typeName, bool isSerialize) 
    {
        if (isSerialize) 
        {
            return ".ConvertToJsonArray()";
        }
        return typeName switch 
        {
            "byte[]" => ".ConvertToArrayByte()",
            "short[]" => ".ConvertToArrayInt16()",
            "int[]" => ".ConvertToArrayInt32()",
            "long[]" => ".ConvertToArrayInt64()",
            "sbyte[]" => ".ConvertToArraySByte()",
            "ushort[]" => ".ConvertToArrayUInt16()",
            "uint[]" => ".ConvertToArrayUInt32()",
            "ulong[]" => ".ConvertToArrayUInt64()",
            "float[]" => ".ConvertToArraySingle()",
            "double[]" => ".ConvertToArrayDouble()",
            "decimal[]" => ".ConvertToArrayDecimal()",
            "char[]" => ".ConvertToArrayChar()",
            "string[]" => ".ConvertToArrayString()",
            "bool[]" => ".ConvertToArrayBoolean()",
            "nint[]" => ".ConvertToArrayIntPtr()",
            "nuint[]" => ".ConvertToArrayUIntPtr()",
            
            _ => ".ConvertToArray()",
        };
    }

    private static string Array2DCheck(string typeName, bool isSerialize) 
    {
        if (isSerialize) 
        {
            return ".ConvertToJsonArray()";
        }
        return typeName switch 
        {
            "byte[,]" => ".ConvertToArrayByte()",
            "short[,]" => ".ConvertToArrayInt16()",
            "int[,]" => ".ConvertToArrayInt32()",
            "long[,]" => ".ConvertToArrayInt64()",
            "sbyte[,]" => ".ConvertToArraySByte()",
            "ushort[,]" => ".ConvertToArrayUInt16()",
            "uint[,]" => ".ConvertToArrayUInt32()",
            "ulong[,]" => ".ConvertToArrayUInt64()",
            "float[,]" => ".ConvertToArraySingle()",
            "double[,]" => ".ConvertToArrayDouble()",
            "decimal[,]" => ".ConvertToArrayDecimal()",
            "char[,]" => ".ConvertToArrayChar()",
            "string[,]" => ".ConvertToArrayString()",
            "bool[,]" => ".ConvertToArrayBoolean()",
            "nint[,]" => ".ConvertToArrayIntPtr()",
            "nuint[,]" => ".ConvertToArrayUIntPtr()",
            
            _ => ".ConvertToArray2D()",
        };
    }

    private static string ListCheck(string typeName, bool isSerialize) 
    {
        if (isSerialize) 
        {
            return ".ConvertToJsonArray()";
        }
        return typeName switch 
        {
            nameof(Byte) => ".ConvertToListByte()",
            nameof(Int16) => ".ConvertToListInt16()",
            nameof(Int32) => ".ConvertToListInt32()",
            nameof(Int64) => ".ConvertToListInt64()",
            nameof(SByte) => ".ConvertToListSByte()",
            nameof(UInt16) => ".ConvertToListUInt16()",
            nameof(UInt32) => ".ConvertToListUInt32()",
            nameof(UInt64) => ".ConvertToListUInt64()",
            nameof(Single) => ".ConvertToListSingle()",
            nameof(Double) => ".ConvertToListDouble()",
            nameof(Decimal) => ".ConvertToListDecimal()",
            nameof(Char) => ".ConvertToListChar()",
            nameof(String) => ".ConvertToListString()",
            nameof(Boolean) => ".ConvertToListBoolean()",
            nameof(IntPtr) => ".ConvertToListIntPtr()",
            nameof(UIntPtr) => ".ConvertToListUIntPtr()",
            
            _ => ".ConvertToList()",
        };
    }
}

