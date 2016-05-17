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
        public void ConvertJsonTest_Template()
        {
            var result = JsonParser.ConvertJson("template.json");
            //var result2 = Program.ConvertJsonObject("template.json");
            
            WriteJsonLog(result);
        }

        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\simpleWindows.json")]
        [TestMethod]
        public void ConvertJsonTest_SimpleWindows()
        {
            var result = JsonParser.ConvertJson("simpleWindows.json");

            WriteJsonLog(result);
        }

        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\201-vm-custom-script-windows.json")]
        [TestMethod]
        public void ConvertJsonTest_VmCustomScriptWindows()
        {
            var result = JsonParser.ConvertJson("201-vm-custom-script-windows.json");

            WriteJsonLog(result);
        }

        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\201-1-vm-loadbalancer-2-nics.json")]
        [TestMethod]
        public void ConvertJsonTest_1VmLoadbalancer2Nics()
        {
            var result = JsonParser.ConvertJson("201-1-vm-loadbalancer-2-nics.json");

            WriteJsonLog(result);
        }

        [Owner("tatsuy")]
        [DeploymentItem(@".\Resources\TestCase\Templates\201-2-vms-internal-load-balancer.json")]
        [TestMethod]
        public void ConvertJsonTest_2VmsInternalLoadbalancer()
        {
            var result = JsonParser.ConvertJson("201-2-vms-internal-load-balancer.json");

            WriteJsonLog(result);
        }

        private void WriteJsonLog(RootObject root)
        {
            var count = 0;
            foreach (var item in root.Resources)
            {
                Console.WriteLine($"{Environment.NewLine}Resource[{count++}]:");
                WriteResourceLog(item);
            }

            JsonParser.OutputJsonFile(root);
        }

        private void WriteResourceLog(Resource resource)
        {
            Console.WriteLine($"{nameof(resource.ApiVersion)}\t: {resource.ApiVersion}");
            Console.WriteLine($"{nameof(resource.Location)}\t: {resource.Location}");
            Console.WriteLine($"{nameof(resource.Name)}\t: {resource.Name}");
            Console.WriteLine($"{nameof(resource.Type)}\t: {resource.Type}");
            WritePropertiesLog(resource.properties, 1);
        }

        private void WritePropertiesLog(List<Property> properties, int n)
        {
            var space = String.Empty;
            var count = 0;
            for (int i = 0; i < n; i++) space += "   ";
            foreach (var property in properties)
            {
                Console.WriteLine($"{space}Property[{count++}]:");
                if (property.Name != null)
                    Console.WriteLine($"{space}{nameof(property.Name)}\t: {property.Name}");
                if(property.Value != null)
                    Console.WriteLine($"{space}{nameof(property.Value)}\t: {property.Value}");
                if(property.Properties != null && property.Properties.Count > 0)
                    WritePropertiesLog(property.Properties, n + 1);
                if(property.Subnets != null && property.Subnets.Count > 0)
                    WriteSubnetsLog(property.Subnets, n + 1);
            }
        }

        private void WriteSubnetsLog(List<Subnet> subnets, int n)
        {
            var space = String.Empty;
            var count = 0;
            for (int i = 0; i < n; i++) space += "   ";
            foreach (var subnet in subnets)
            {
                Console.WriteLine($"{space}Subnet[{count++}]:");
                Console.WriteLine($"{space}{nameof(subnet.Name)}\t: {subnet.Name}");
                Console.WriteLine($"{space}{nameof(subnet.AddressPrefix)}\t: {subnet.AddressPrefix}");
                Console.WriteLine($"{space}{nameof(subnet.NetworkSecurityGroup)}\t: {subnet.NetworkSecurityGroup}");
            }
        }
    }
}
