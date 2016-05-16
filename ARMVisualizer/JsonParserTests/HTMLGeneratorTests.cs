using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLVisualizer;

namespace JsonParserTests
{
    /// <summary>
    /// HTMLGeneratorTests の概要の説明
    /// </summary>
    [TestClass]
    public class HTMLGeneratorTests
    {
        [Owner("tatsuy")]
        [TestMethod]
        public void CreateHTML()
        {
            StringBuilder result = new StringBuilder();
            result.Append(HTMLGenerator.WriteHeader());

            int Xbase = 50;
            int YBase = 50;


            // VNET --------------------------------------
            result.Append(HTMLGenerator.WriteVNet(Xbase - 25, YBase - 25, 1450, 1050));



            // Subnet 1 ---------------------------
            result.Append(HTMLGenerator.WriteSubNet(Xbase + 10, YBase + 10, 950, networkSecurityGroup:true, loadBalancer:true, availabilitySet:true));

            result.Append(HTMLGenerator.WriteLoadBalancer(Xbase + 50, YBase + 245));

            result.Append(HTMLGenerator.WriteAvailabilitySet(Xbase + 160, YBase + 50, 490));
            result.Append(HTMLGenerator.WriteVM(Xbase + 220, YBase + 100, 230));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 135, networkSecurityGroup:true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 170));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 205));

            result.Append(HTMLGenerator.WriteVM(Xbase + 220, YBase + 350, 180));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 385, networkSecurityGroup: true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 420));

            result.Append(HTMLGenerator.WriteVM(Xbase + 220, YBase + 570, 180));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 605));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 640));

            result.Append(HTMLGenerator.WriteVM(Xbase + 220, YBase + 780, 145));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 815));

            // Subnet 2 ---------------------------
            result.Append(HTMLGenerator.WriteSubNet(Xbase + 560, YBase + 10, 530));

            result.Append(HTMLGenerator.WriteVM(Xbase + 620, YBase + 80, 230));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 115, networkSecurityGroup:true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 150));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 185));

            result.Append(HTMLGenerator.WriteVM(Xbase + 620, YBase + 330, 180));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 365, networkSecurityGroup: true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 400));


            // Subnet 3 ---------------------------  
            result.Append(HTMLGenerator.WriteSubNet(Xbase + 960, YBase + 10, 550, networkSecurityGroup: true, availabilitySet: true));
            result.Append(HTMLGenerator.WriteAvailabilitySet(Xbase + 1010, YBase + 50, 490));

            result.Append(HTMLGenerator.WriteVM(Xbase + 1070, YBase + 100, 230));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 1080, YBase + 135, networkSecurityGroup: true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 1080, YBase + 170));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 1080, YBase + 205));

            result.Append(HTMLGenerator.WriteVM(Xbase + 1070, YBase + 350, 180));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 1080, YBase + 385, networkSecurityGroup: true));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 1080, YBase + 420));

            result.Append(HTMLGenerator.WriteFooter());
            
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "test.html"), result.ToString());
        }
    }
}
