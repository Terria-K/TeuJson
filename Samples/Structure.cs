using System;
using System.Collections.Generic;
using System.Numerics;
using TeuJson;
using TeuJson.Attributes;
using Utility;

namespace Structural;

[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Structure 
{
    [Name("array")]
    public int[]? Array { get; set; }
    public int Number { get; set; }
    public string? Text { get; set; }
    [Name("falsy")]
    public bool TrueOrFalse { get; set; }
    [Name("otherName")]
    [TeuObject]
    public string? Field;
    [Custom(CustomConverters.Use)]
    public TimeSpan Span { get; set; }
    [Custom(CustomConverters.Use)]
    public List<Vector2>? Position { get; set; }
}