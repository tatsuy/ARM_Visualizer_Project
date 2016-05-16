using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonParser;

namespace ARMVisualizerTests
{
    [TestClass]
    public class JsonParserTests
    {
        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\template.json")]
        [TestMethod]
        public void ConvertJsonTest()
        {
            var result = JsonParser.Program.ConvertJson("template.json");
            var count = 0;
            foreach (var item in result.resources)
            {
                Console.WriteLine($"{Environment.NewLine}Resource[{count++}]:");
                WriteResourceLog(item);
            }

            JsonParser.Program.OutputJsonFile(result);
        }

        private void WriteResourceLog(Resource resource)
        {
            Console.WriteLine($"{nameof(resource.apiVersion)}\t: {resource.apiVersion}");
            Console.WriteLine($"{nameof(resource.location)}\t: {resource.location}");
            Console.WriteLine($"{nameof(resource.name)}\t: {resource.name}");
            Console.WriteLine($"{nameof(resource.type)}\t: {resource.type}");
        }
    }
}
