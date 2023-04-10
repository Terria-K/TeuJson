using System;
namespace TeuJson.Generator;

partial class TeuJsonGenerator
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
            
            _ => $".ConvertToArray<{typeName.Replace("[]", "")}>()",
        };
    }

    private static string Array2DCheck(string typeName, bool isSerialize) 
    {
        if (isSerialize) 
        {
            return ".ConvertToJsonArray2D()";
        }
        return typeName switch 
        {
            "byte[*,*]" => ".ConvertToArrayByte2D()",
            "short[*,*]" => ".ConvertToArrayInt162D()",
            "int[*,*]" => ".ConvertToArrayInt322D()",
            "long[*,*]" => ".ConvertToArrayInt642D()",
            "sbyte[*,*]" => ".ConvertToArraySByte2D()",
            "ushort[*,*]" => ".ConvertToArrayUInt162D()",
            "uint[*,*]" => ".ConvertToArrayUInt322D()",
            "ulong[*,*]" => ".ConvertToArrayUInt642D()",
            "float[*,*]" => ".ConvertToArraySingle2D()",
            "double[*,*]" => ".ConvertToArrayDouble2D()",
            "decimal[*,*]" => ".ConvertToArrayDecimal2D()",
            "char[*,*]" => ".ConvertToArrayChar2D()",
            "string[*,*]" => ".ConvertToArrayString2D()",
            "bool[*,*]" => ".ConvertToArrayBoolean2D()",
            "nint[*,*]" => ".ConvertToArrayIntPtr2D()",
            "nuint[*,*]" => ".ConvertToArrayUIntPtr2D()",
            
            _ => $".ConvertToArray2D<{typeName.Replace("[*,*]", "")}>()",
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
            nameof(Byte) or "byte" => ".ConvertToListByte()",
            nameof(Int16) or "short" => ".ConvertToListInt16()",
            nameof(Int32) or "int" => ".ConvertToListInt32()",
            nameof(Int64) or "long" => ".ConvertToListInt64()",
            nameof(SByte) or "sbyte" => ".ConvertToListSByte()",
            nameof(UInt16) or "ushort" => ".ConvertToListUInt16()",
            nameof(UInt32) or "uint" => ".ConvertToListUInt32()",
            nameof(UInt64) or "ulong" => ".ConvertToListUInt64()",
            nameof(Single) or "float" => ".ConvertToListSingle()",
            nameof(Double) or "double" => ".ConvertToListDouble()",
            nameof(Decimal) or "decimal" => ".ConvertToListDecimal()",
            nameof(Char) or "char" => ".ConvertToListChar()",
            nameof(String) or "string" => ".ConvertToListString()",
            nameof(Boolean) or "bool" => ".ConvertToListBoolean()",
            nameof(IntPtr) or "nint" => ".ConvertToListIntPtr()",
            nameof(UIntPtr) or "nuint" => ".ConvertToListUIntPtr()",
            
            _ => $".ConvertToList<{typeName}>()",
        };
    }
}

