using System.Numerics;
using TeuJson.Attributes;
using Xunit;

namespace TeuJson.Tests;

public partial class Human : IDeserialize, ISerialize
{
    public string? Name { get; set; }
    
    [Custom("TeuJson.Tests.TestConverter", Write = "Vector2ToJson")]
    public Vector2 Position { get; set; }
}

public partial class SuperHuman : Human, IDeserialize, ISerialize 
{
    public int Damage { get; set; }
    public SuperPowers[]? Powers { get; set; }
}

public enum SuperPowers 
{
    Fireball,
    IceSpike,
    Flying,
    SpinWeb
}

public static class TestConverter 
{
    public static Vector2 ToVector2(this JsonValue value) 
    {
        if (value.IsNull)
            return Vector2.Zero;

        return new Vector2(value["x"], value["y"]);
    }

    public static JsonValue Vector2ToJson(this Vector2 value) 
    {
        return new JsonObject 
        {
            ["x"] = value.X,
            ["y"] = value.Y
        };
    }
}


public class AdvanceSerializationTest 
{
    [Fact]
    public void ShouldSerializeWithConverter() 
    {
        Vector2 expectedPosition = new Vector2(30, 40);
        var json = """
        {
            "Name": "Peter Parker",
            "Position": {
                "x": 30,
                "y": 40
            }
        }
        """;

        var jsonObj = JsonTextReader.FromText(json);
        var human = JsonConvert.Deserialize<Human>(jsonObj);

        Assert.True(jsonObj.IsObject);
        Assert.Equal(2, jsonObj.Count);

        Assert.Equal("Peter Parker", human.Name);
        Assert.Equal(expectedPosition, human.Position);
    }

    [Fact]
    public void ShouldDeserializeWithConverter() 
    {
        var human = new Human 
        {
            Name = "Peter Parker",
            Position = new Vector2(45, 20)
        };

        var jsonObj = JsonConvert.Serialize(human);
        var jsonObjText = jsonObj.ToString(new JsonTextWriterOptions { Minimal = true });

        Assert.True(jsonObj.IsObject);
        Assert.Equal(2, jsonObj.Count);

        Assert.Equal("Peter Parker", jsonObj["Name"]);
        Assert.Equal<float>(45, jsonObj["Position"]["x"]);
        Assert.Equal<float>(20, jsonObj["Position"]["y"]);

        Assert.Equal("""{"Name": "Peter Parker","Position": {"x": 45,"y": 20}}""", jsonObjText);
    }

    [Fact]
    public void ShouldDeserializeDerivedClass() 
    {
        var expectedPosition = new Vector2(50, 256);
        var expectedPowers = new SuperPowers[2] { SuperPowers.Flying, SuperPowers.SpinWeb };
        var json = """
        {
            "Name": "Peter Parker",
            "Position": {
                "x": 50,
                "y": 256
            },
            "Damage": 20,
            "Powers": ["Flying", "SpinWeb"]
        }
        """;
        var jsonObj = JsonTextReader.FromText(json);
        var superHuman = JsonConvert.Deserialize<SuperHuman>(jsonObj);

        Assert.True(jsonObj.IsObject);
        Assert.Equal(4, jsonObj.Count);

        Assert.Equal("Peter Parker", superHuman.Name);
        Assert.Equal(expectedPosition, superHuman.Position);
        Assert.Equal(20, superHuman.Damage);
        Assert.Equal(expectedPowers, superHuman.Powers);
    }

    [Fact]
    public void ShouldSerializeDerivedClass() 
    {
        var expectedPowers = new SuperPowers[2] { SuperPowers.Fireball, SuperPowers.Flying };
        var superHuman = new SuperHuman 
        {
            Name = "Tony Stark",
            Position = new Vector2(150, 400),
            Damage = 100,
            Powers = new SuperPowers[2] { SuperPowers.Fireball, SuperPowers.Flying }
        };

        var jsonObj = JsonConvert.Serialize(superHuman);
        var jsonObjText = jsonObj.ToString(new JsonTextWriterOptions { Minimal = true });

        Assert.True(jsonObj.IsObject);
        Assert.Equal(4, jsonObj.Count);

        Assert.Equal("Tony Stark", jsonObj["Name"]);
        Assert.Equal<float>(150, jsonObj["Position"]["x"]);
        Assert.Equal<float>(400, jsonObj["Position"]["y"]);
        Assert.Equal<int>(100, jsonObj["Damage"]);
        Assert.Equal(SuperPowers.Fireball, (SuperPowers)(int)jsonObj["Powers"][0]);
        Assert.Equal(SuperPowers.Flying, (SuperPowers)(int)jsonObj["Powers"][1]);

        Assert.Equal("""{"Name": "Tony Stark","Position": {"x": 150,"y": 400},"Damage": 100,"Powers": [0, 2]}""", jsonObjText);
    }
}