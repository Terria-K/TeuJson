using TeuJson.Attributes;
using Xunit;

namespace TeuJson.Tests 
{
    public sealed partial class Person : ISerialize, IDeserialize
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public int[]? FavoriteNumbers { get; set; }
        [Ignore]
        public string? City { get; set; }
    }

    public partial struct Block : ISerialize, IDeserialize
    {
        [TeuObject]
        public int Health;

        [TeuObject]
        public float Velocity;

        [TeuObject]
        public Durability Durability;
    }

    public enum Durability 
    {
        Strong,
        Normal,
        Weak
    }

    public class SerializationTest 
    {
        [Fact]
        public void ShouldDeserializeJsonText() 
        {
            var json = """
            {
                "name": "Gorden Lockmen",
                "age": 17
            }
            """;

            var jsonObj = JsonTextReader.FromText(json);
            Assert.True(jsonObj.IsObject);
            Assert.Equal(2, jsonObj.Count);
            Assert.True(jsonObj.Contains("name"));

            Assert.Equal("Gorden Lockmen", jsonObj["name"]);
            Assert.Equal<int>(17, jsonObj["age"]);
        }

        [Fact]
        public void ShouldDeserializeJsonFile() 
        {
            var jsonObj = JsonTextReader.FromFile("res/person.json");
            Assert.True(jsonObj.IsObject);
            Assert.Equal(2, jsonObj.Count);
            Assert.True(jsonObj.Contains("name"));

            Assert.Equal("Gorden Lockmen", jsonObj["name"]);
            Assert.Equal<int>(17, jsonObj["age"]);
        }

        [Fact]
        public void ShouldDeserializeJsonTextWithTypes() 
        {
            var json = """
            {
                "Name": "Gorden Lockmen",
                "Age": 17,
                "City": "Hot Mesa",
                "FavoriteNumbers": [2, 5, 1, 18]
            }
            """;

            var jsonObj = JsonTextReader.FromText(json);
            var person = JsonConvert.Deserialize<Person>(jsonObj);
            Assert.True(jsonObj.IsObject);
            Assert.Equal(4, jsonObj.Count);
            Assert.True(jsonObj.Contains("Name"));

            Assert.Equal("Gorden Lockmen", person.Name);
            Assert.Equal(17, person.Age);
            Assert.Null(person.City);

            Assert.True(jsonObj.Contains("FavoriteNumbers"));
            Assert.True(jsonObj["FavoriteNumbers"].IsArray);
            Assert.Equal(4, jsonObj["FavoriteNumbers"].Count);
            Assert.Equal<int>(1, jsonObj["FavoriteNumbers"][2]);
        }

        [Fact]
        public void ShouldDeserializeJsonFileWithTypes() 
        {
            var jsonObj = JsonTextReader.FromFile("res/PersonClass.json");
            var person = JsonConvert.Deserialize<Person>(jsonObj);


            Assert.True(jsonObj.IsObject);
            Assert.Equal(4, jsonObj.Count);
            Assert.True(jsonObj.Contains("Name"));

            Assert.Equal("Gorden Lockmen", person.Name);
            Assert.Equal(17, person.Age);
            Assert.Null(person.City);

            Assert.True(jsonObj.Contains("FavoriteNumbers"));
            Assert.True(jsonObj["FavoriteNumbers"].IsArray);
            Assert.Equal(4, jsonObj["FavoriteNumbers"].Count);
            Assert.Equal<int>(1, jsonObj["FavoriteNumbers"][2]);
        }

        [Fact]
        public void ShouldSerializeClass() 
        {
            var person = new Person 
            {
                Name = "Barney Dinosaur",
                Age = 20,
                City = "Loco",
                FavoriteNumbers = new int[4] { 2, 1, 4, 20 }
            };

            var jsonObj = JsonConvert.Serialize(person);
            Assert.True(jsonObj.IsObject);
            Assert.Equal(3, jsonObj.Count);
            Assert.True(jsonObj.Contains("Name"));
            Assert.False(jsonObj.Contains("City"));

            Assert.Equal("Barney Dinosaur", jsonObj["Name"]);
            Assert.Equal<int>(20, jsonObj["Age"]);
            var jsonObjText = jsonObj.ToString();

            Assert.True(jsonObj.Contains("FavoriteNumbers"));
            Assert.True(jsonObj["FavoriteNumbers"].IsArray);
            Assert.Equal(4, jsonObj["FavoriteNumbers"].Count);
            Assert.Equal<int>(4, jsonObj["FavoriteNumbers"][2]);

            Assert.Equal("""{"Name": "Barney Dinosaur","Age": 20,"FavoriteNumbers": [2, 1, 4, 20]}""", jsonObjText);
        }

        [Fact]
        public void ShouldSerializeStruct() 
        {
            var weakBlock = new Block
            {
                Health = 50,
                Velocity = 2.5f,
                Durability = Durability.Weak
            };

            var strongBlock = new Block
            {
                Health = 100,
                Velocity = 0.5f,
                Durability = Durability.Strong
            };

            var weakBlockJson = JsonConvert.Serialize(weakBlock);
            var strongBlockJson = JsonConvert.Serialize(strongBlock);

            Assert.True(weakBlockJson.IsObject);
            Assert.True(strongBlockJson.IsObject);

            Assert.Equal(3, weakBlockJson.Count);
            Assert.Equal(3, strongBlockJson.Count);
            
            Assert.True(weakBlockJson.Contains("Health"));
            Assert.True(strongBlockJson.Contains("Health"));

            Assert.Equal<int>(50, weakBlockJson["Health"]);
            Assert.Equal<int>(100, strongBlockJson["Health"]);

            Assert.Equal<float>(2.5f, weakBlockJson["Velocity"]);
            Assert.Equal<float>(0.5f, strongBlockJson["Velocity"]);

            Assert.Equal(Durability.Weak, (Durability)weakBlockJson["Durability"].AsInt32);
            Assert.Equal(Durability.Strong, (Durability)strongBlockJson["Durability"].AsInt32);

            var weakBlockJsonText = weakBlockJson.ToString();
            var strongBlockJsonText = strongBlockJson.ToString();

            Assert.Equal("""{"Health": 50,"Velocity": 2.5,"Durability": 2}""", weakBlockJsonText);
            Assert.Equal("""{"Health": 100,"Velocity": 0.5,"Durability": 0}""", strongBlockJsonText);
        }
    }
}