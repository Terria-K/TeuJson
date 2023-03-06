using System;
using System.IO;

using var stream = File.OpenRead("Jsontester.json");
using var jsonReader = new JsonT.JsonTextReader(stream);
jsonReader.ReadObject();

Console.ReadLine();