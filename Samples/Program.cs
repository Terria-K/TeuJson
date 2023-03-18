using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Structural;
using TeuJson;

var reader = JsonTextReader.FromFile("Samples/res/Jsontester.json");


JsonTextWriter.WriteToFile("Samples/res/Jsontesterwrite.json", reader);

var readingStructure = JsonTextReader.FromFile("Samples/res/JsonStructure.json");
var structure = JsonConvert.Deserialize<Structure>(readingStructure);

var numberToAssert = new int[4] { 2, 3, 1, 5};

if (structure.Array is not null) 
{
    for (int i = 0; i < structure.Array.Length; i++) 
    {
        Debug.Assert(numberToAssert[i] == structure.Array[i]);
    }
} 

Debug.Assert(structure.Number == 4);
Debug.Assert(structure.Text == "hello_world");
Debug.Assert(!structure.TrueOrFalse);
Debug.Assert(structure.Field == "This is my field");

structure.Positions = new List<Vector2>() { new Vector2(0, 2), new Vector2(25, 10)};
structure.Vec4 = new Structural.Vector4();
structure.NumberEnum = Enumeration.B;

var structureObject = JsonConvert.Serialize(structure);

JsonTextWriter.WriteToFile("Samples/res/JsonStructurewrite.json", structureObject);
JsonTextWriter.WriteToFile("Samples/res/JsonStructurewriteminimal.json", structureObject, new JsonTextWriterOptions {
    Minimal = true
});

JsonBinaryWriter.WriteToFile("Samples/res/JsonStructure.bin", structureObject);

var readingStructureBin = JsonBinaryReader.FromFile("Samples/res/JsonStructure.bin");
var structureBin = JsonConvert.Deserialize<Structure>(readingStructureBin);

if (structureBin.Array is not null) 
{
    for (int i = 0; i < structureBin.Array.Length; i++) 
    {
        Debug.Assert(numberToAssert[i] == structureBin.Array[i]);
    }
} 

Debug.Assert(structureBin.Number == 4);
Debug.Assert(structureBin.Text == "hello_world");
Debug.Assert(!structureBin.TrueOrFalse);
Debug.Assert(structureBin.Field == "This is my field");
Console.ReadLine();