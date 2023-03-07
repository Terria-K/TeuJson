using System.IO;

using var stream = File.OpenRead("Jsontester.json");
using var jsonReader = new JsonT.JsonTextReader(stream);
var value = jsonReader.ReadObject();



unsafe {
    int val = 2;
    int* ptr = &val;

    nint nPtr = (nint)ptr;
    value["pointer"] = nPtr;
}

JsonT.JsonTextWriter.WriteToFile("Jsontesternew.json", value);