using System;

namespace JsonT.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class JsonTSerializableAttribute : Attribute 
{
    public bool Serializable 
    {
        get => serialization;
        set => serialization = value;
    }
    public bool Deserializable 
    {
        get => deserialization;
        set => deserialization = value;
    }
    private bool deserialization = true;
    private bool serialization;


    public JsonTSerializableAttribute() {}
}
