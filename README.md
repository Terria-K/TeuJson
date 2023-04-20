# TeuJson
A Reflection-less and Lightweight Json Library using source generator.

[![Nuget](https://img.shields.io/nuget/v/TeuJson?style=for-the-badge)](https://www.nuget.org/packages/TeuJson/)

### Installation
Install these two required packages.

```console
dotnet add package TeuJson --version 3.1.2
dotnet add package TeuJson.Generator --version 3.1.2
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
// Unlike most libraries, TeuJson uses interfaces instead of an attribute on a type.
// This is much cleaner way to specify the type if it can serialized or deserialized.
public partial class Person : IDeserialize, ISerialize
{
    [Name("name")]
    public string Name { get; set; }
    public int Age { get; set; }
    [Ignore]
    public string Location { get; set; }
    [TeuObject]
    public string City;
}

// Then use it like this:
var person = new Person { 
  Name = "John Anthony",
  Age = 32,
  Location = "North Pole",
  City = "Santa's City"
};
var serialized = JsonConvert.Serialize(person);
JsonTextWriter.WriteToFile("person.json", person);

var johnJson = JsonTextReader.FromFile("person.json");
var john = JsonConvert.Deserialize<Person>(johnJson);
```

The output of the file will be:

```json
{
  "name": "John Anthony",
  "Age": 32,
  "City": "Santa's City"
}
```

###  What it generates?
It generates the code like what you've expected, there might be a special cases with classes since they can have null values. All of Serializable classes will use a fully qualified name to instantiate themselves. 

The reason why the methods are `virtual` is because the class might have a derived class which can also be a serializable to override those methods. If you don't want this, mark the `class` as `sealed`. Structs do not have inheritance, so it won't have a `virtual` method.

```c#
// Source Generated code
using TeuJson;

partial class Person
{
    public virtual void Deserialize(JsonObject @__obj)
    {
        Name = @__obj["name"];
        Age = @__obj["Age"];
        City = @__obj["City"];
    }
}

partial class Rect4
{
    public virtual JsonObject Serialize()
    {
        var __builder = new JsonObject();
        __builder["name"] = Name;
        __builder["Age"] = Age;
        __builder["City"] = City;
        return __builder;
    }
}
```

## Custom Converters

You can create your own converters by defining a function inside of a static class. You will not necessarily needed if you have an access to that specific class or struct.

The way to declare it is differs from NET 6 and NET 7.
```C#
// Creating the converter
/* MyMathConverter.cs */
namespace Maths;

/* NET 6.0 and below */
/** public class MyMathConverter **/
public class MyMathConverter 
{
// Converters are named sensitive, it must follow the naming convetion in order to work.
// Writer = JsonValue ToJson(this <T> value);
// Reader = <T> To<T>(this JsonValue value);

    // Extensions are possible in NET 6, but not in NET 7 due to its limitation.
    public static JsonValue ToJson(Vector2 value) 
    {
        // Json object is similar to Dictionary.
        return new JsonObject 
        {
            ["x"] = value.X,
            ["y"] = value.Y
        };
    }

    public static Vector2 ToVector2(JsonValue value) 
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
public sealed partial class Player : IDeserialize
{
    [Name("name")]
    public string Name { get; set; }
    /* NET 6 and below: Name must be fully qualified*/
    /** Name must be fully qualified **/
    /** [Custom("Maths.MyMathConverter")] **/
    [Custom<MyMathConverter>()]
    [Name("position")]
    public Vector2 Position { get; set;}
}
```

# License

MIT License ([Read License](LICENSE.txt)).
