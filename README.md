# TeuJson
A Reflection-less and Lightweight Json Library using source generator.

[![Nuget](https://img.shields.io/nuget/v/TeuJson?style=for-the-badge)](https://www.nuget.org/packages/TeuJson/)

### Installation
Install these two required packages.

```console
dotnet add package TeuJson --version 1.0.2
dotnet add package TeuJson.Generator --version 1.0.3
```

## Features

- Serializers and deserializers.
- 2 formatting options (Pretty, and Minimal).
- Custom converters defined as functions.
- Allows trailing commas.
- Read and write as binary.

# Usage

## Creating a class with a Serializable.

```C#
// Must be explicitly enabled one of the property or else it wouldn't work.
// If Serializable is false, you can only deserialize the class.
[TeuJsonSerializable(Deserializable = true, Serializable = true)]
public partial class Person 
{
    [Name("name")]
    public string Name { get; set; }
    public int Age { get; set; }
    [Ignore]
    public string Location { get; set; }
    [TObject]
    public string City;
}

// Then use it like this:
var person = new Person { 
  Name = "John Anthony",
  Age = 32,
  Location = "North Pole",
  City = "Santa's City"
};
var serialized = person.Serialize();
JsonTextWriter.WriteToFile("person.json", person);

var johnJson = JsonTextReader.FromFile("person.json");
var john = new Person();
john.Deserialize(johnJson.AsJsonObject);
```

The output of the file will be:

```json
{
  "name": "John Anthony",
  "Age": 32,
  "City": "Santa's City"
}
```

## Custom Converters

You can create your own converters by defining a function inside of a static class. You will not necessarily needed if you have an access to that specific class or struct.

```C#
// Creating the converter
/* MyMathConverter.cs */
namespace Maths;
public static class MyMathConverter 
{
// Converters are named sensitive, it must follow the naming convetion in order to work.
// Writer = JsonValue ToJson(this <T> value);
// Reader = <T> To<T>(this JsonValue value);

    public static JsonValue ToJson(this Vector2 value) 
    {
        // Json object is similar to Dictionary.
        return new JsonObject 
        {
            ["x"] = value.X,
            ["y"] = value.Y
        };
    }

    public static Vector2 ToVector2(this JsonValue value) 
    {
        // check if the json value is object
        if (value.IsObject) 
        {
            int x = value["x"];
            int y = value["y"];
            return new Vector2(x, y);
        }
        return Vector2.Zero;
    }
}
```

```C#
[TeuJsonSerializable(Deserializable = true)]
public partial class Player 
{
    [Name("name")]
    public string Name { get; set; }
    // Name must be fully qualified
    [Custom("Maths.MyMathConverter")]
    [Name("position")]
    public Vector2 Position { get; set;}
}
```

# License

MIT License ([Read License](LICENSE.txt)).
