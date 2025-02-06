using Heos.Tools.ClassGenerator.Generator;
using Heos.Tools.XmlParser;


var path = args[0];
var @namespace = args[1];
var xmlSpecPath = args[2];

var parser = new SpecParser(xmlSpecPath);
var generator = new FileGenerator();

var requests = parser.Parse();
await generator.Generate(path, @namespace, requests, parser.GetSpecificationInfo());

Console.WriteLine();