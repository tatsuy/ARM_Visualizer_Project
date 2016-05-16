using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLVisualizer;

namespace ARMVisualizerTests
{
    [TestClass]
    public class JsonParserTests
    {
        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\template.json")]
        [TestMethod]
        public void ConvertJsonTest1()
        {
            var result = JsonParser.ConvertJson("template.json");
            //var result2 = Program.ConvertJsonObject("template.json");
            
            WriteJsonLog(result);
        }

        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\simpleWindows.json")]
        [TestMethod]
        public void ConvertJsonTest2()
        {
            var result = JsonParser.ConvertJson("simpleWindows.json");

            WriteJsonLog(result);
        }

        private void WriteJsonLog(RootObject root)
        {
            var count = 0;
            foreach (var item in root.Resources)
            {
                Console.WriteLine($"{Environment.NewLine}Resource[{count++}]:");
                WriteResourceLog(item, root.Parameters);
            }

            JsonParser.OutputJsonFile(root);
        }

        private void WriteResourceLog(Resource resource, List<Parameter> parameters)
        {
            Console.WriteLine($"{nameof(resource.ApiVersion)}\t: {resource.ApiVersion}");
            Console.WriteLine($"{nameof(resource.Location)}\t: {resource.Location}");
            Console.WriteLine($"{nameof(resource.Name)}\t: {resource.Name}");
            Console.WriteLine($"{nameof(resource.Type)}\t: {resource.Type}");
        }
    }
}
