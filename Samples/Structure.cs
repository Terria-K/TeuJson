using JsonT;
using JsonT.Attributes;

[JsonTSerializable(Deserializable = true, Serializable = true)]
public partial class Structure 
{
    [Name("array")]
    public int[]? Array { get; set; }

    public int Number { get; set; }
    public string? Text { get; set; }
    [Name("falsy")]
    public bool TrueOrFalse { get; set; }
    [Name("otherName")]
    [TObject]
    public string? Field;
}