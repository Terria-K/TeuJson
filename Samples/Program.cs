using System;
using System.Diagnostics;
using Structural;
using TeuJson;

var reader = JsonTextReader.FromFile("Samples/res/Jsontester.json");

var minimalReader = JsonTextReader.FromFile("Samples/res/JsontesterMinimal.json");


JsonTextWriter.WriteToFile("Samples/res/Jsontesterwrite.json", reader);

var readingStructure = JsonTextReader.FromFile("Samples/res/JsonStructure.json");
var structure = new Structure();
structure.Deserialize(readingStructure.AsJsonObject);

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

var structureObject = structure.Serialize();

JsonTextWriter.WriteToFile("Samples/res/JsonStructurewrite.json", structureObject);

JsonBinaryWriter.WriteToFile("Samples/res/JsonStructure.bin", structureObject);

var readingStructureBin = JsonBinaryReader.FromFile("Samples/res/JsonStructure.bin");
var structureBin = new Structure();
structure.Deserialize(readingStructureBin.AsJsonObject);

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