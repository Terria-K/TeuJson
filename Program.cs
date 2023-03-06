using System;
using System.IO;

using var stream = File.OpenRead("Jsontester.json");
using var jsonReader = new JsonT.JsonTextReader(stream);
var value = jsonReader.ReadObject();

Console.WriteLine(value.ToString());

Console.ReadLine();