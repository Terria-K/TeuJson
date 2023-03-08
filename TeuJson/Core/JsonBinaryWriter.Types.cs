namespace TeuJson;

public partial class JsonBinaryWriter 
{
    public override void WriteValue(byte value)
    {
        writer.Write((byte)JsonHint.Byte);
        writer.Write(value);
    }
    public override void WriteValue(short value)
    {
        writer.Write((byte)JsonHint.Int16);
        writer.Write(value);
    }
    public override void WriteValue(int value)
    {
        writer.Write((byte)JsonHint.Int32);
        writer.Write(value);
    }
    public override void WriteValue(long value)
    {
        writer.Write((byte)JsonHint.Int64);
        writer.Write(value);
    }
    public override void WriteValue(sbyte value)
    {
        writer.Write((byte)JsonHint.SByte);
        writer.Write(value);
    }
    public override void WriteValue(ushort value)
    {
        writer.Write((byte)JsonHint.UInt16);
        writer.Write(value);
    }
    public override void WriteValue(uint value)
    {
        writer.Write((byte)JsonHint.UInt32);
        writer.Write(value);
    }
    public override void WriteValue(ulong value)
    {
        writer.Write((byte)JsonHint.UInt64);
        writer.Write(value);
    }
    public override void WriteValue(float value)
    {
        writer.Write((byte)JsonHint.Single);
        writer.Write(value);
    }
    public override void WriteValue(double value)
    {
        writer.Write((byte)JsonHint.Double);
        writer.Write(value);
    }
    public override void WriteValue(bool value)
    {
        writer.Write((byte)JsonHint.Boolean);
        writer.Write(value);
    }
    public override void WriteValue(decimal value)
    {
        writer.Write((byte)JsonHint.Decimal);
        writer.Write(value);
    }
    public override void WriteValue(char value)
    {
        writer.Write((byte)JsonHint.Char);
        writer.Write(value);
    }
    public override void WriteValue(nint value)
    {
        writer.Write((byte)JsonHint.IntPtr);
        writer.Write(value);
    }
    public override void WriteValue(nuint value)
    {
        writer.Write((byte)JsonHint.UIntPtr);
        writer.Write(value);
    }
    
}