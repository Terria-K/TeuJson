using System;
using JsonT;

var reader = JsonTextReader.FromFile("Samples/Jsontester.json");

Console.WriteLine(reader);

var minimalReader = JsonTextReader.FromFile("Samples/JsontesterMinimal.json");

Console.WriteLine(minimalReader);

JsonTextWriter.WriteToFile("Samples/Jsontesterwrite.json", minimalReader);
