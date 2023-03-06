namespace JsonT;

public enum JsonToken 
{
    Null = -1,
    LParent,
    RParent,
    Key,
    LBracket,
    RBracket,
    String,
    Number,
    Boolean,
}