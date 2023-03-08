using System;
using TeuJson;

var reader = JsonTextReader.FromFile("Samples/Jsontester.json");

var minimalReader = JsonTextReader.FromFile("Samples/JsontesterMinimal.json");


JsonTextWriter.WriteToFile("Samples/Jsontesterwrite.json", reader);

var readingStructure = JsonTextReader.FromFile("Samples/JsonStructure.json");
var structure = new Structure();
structure.Deserialize(readingStructure.AsJsonObject);

if (structure.Array is not null)  
    foreach (var arr in structure.Array)
        Console.WriteLine(arr);

Console.WriteLine("Number: " + structure.Number);

Console.WriteLine("Text: " + structure.Text);
Console.WriteLine("TrueOrFalse: " + structure.TrueOrFalse);
Console.WriteLine("Field: " + structure.Field);

var structureObject = structure.Serialize();

JsonTextWriter.WriteToFile("Samples/JsonStructurewrite.json", structureObject);