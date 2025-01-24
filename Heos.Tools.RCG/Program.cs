using Heos.Tools.RCG.Generator;
using Heos.Tools.RCG.Helpers;

const string path = @"C:\Users\march\source\HeosDesktop\Heos.API\Models\HEOS";
const string @namespace = "Heos.API.Models.HEOS";

var parser = new XmlParser();
var generator = new ClassGenerator();

var requests = parser.Parse();
await generator.GenerateFromRequests(path, @namespace, requests);

Console.WriteLine();