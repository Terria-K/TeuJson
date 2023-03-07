namespace JsonT;

public enum JsonHint : byte
{
    Null,
    Char,
    Byte,
    Short,
    Int,
    Long,
    Sbyte,
    UShort,
    UInt,
    ULong,
    Float,
    Double,
    Decimal,
    String,
    Pointer,
    UnsignedPointer,
    Dynamic = 64,
}