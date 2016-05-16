using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTMLVisualizer
{
    public static class HTMLGenerator
    {
        public static string WriteHeader()
        {
            return "<html>\n" +
                   "<body>\n" +
                   "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' width='1500' height='1200' >\n\n";
        }

        public static string WriteFooter()
        {
            return "</svg>\n" +
                   "</body>\n" +
                   "</html>\n\n";
        }

        public static string WriteVNet(int x, int y, int width, int height)
        {
            string vnetName = "Azure Network";
            string vnetPrefix = "10.0.0.0/8";
            return $"<g transform='translate({x} {y})'>\n" +
                   $"<rect x='0' y='-10' width='{width}' height='{height}' fill='#ffffff' stroke='#1e476c' stroke-width='2'></rect>\n" +
                   $"<rect x='0' y='-10' width='300' height='25' fill='#1e476c' stroke='#1e476c' stroke-width='2'></rect>\n" +
                   $"<text style='font-size: 14pt;font-family: consolas;fill : white;' y='8' x='5'>VNet :  {vnetName}</text>\n" +
                   $"<text style='font-size: 14pt;font-family: consolas;fill : #152060;' y='8' x='310'>Prefix : {vnetPrefix}</text>\n" +
                   $"</g>\n\n";
        }

        public static string WriteVM(int x, int y, int height)
        {
            string vmName = "Azure VM";
            string vmType = "Standard G3";

            return $"<g transform='translate({x} {y})'>\n" +
                   $"<rect x='0' y='25' width='250' height='{height}' fill='white'stroke='#152060' stroke-width='2'></rect>\n" +
                   $"<rect x='0' y='-15' width='45' height='40' fill='#ffffff' stroke='#152060' stroke-width='2'></rect>\n" +
                   $"<rect x='45' y='-15' width='205' height='40' fill='#152060' stroke='#152060' stroke-width='2'></rect>\n" +
                   $"<text style='font-size: 11pt;font-family: consolas;fill : white;' y='0' x='50'>Name: {vmName}</text>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : white;' y='17' x='50'>Size: {vmType}</text>\n" +
                   $"<image height='30' width='30' x='8' y='-10' xlink:href='icons/s_VM-symbol-only.png'></image>\n" +
                   $"<line stroke='#152060' stroke-width='1' x1='5' x2='240' y1='{height - 70}' y2='{height - 70}'/>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : #152060;' y='{height - 55}' x='10'>Prop01: Sample Property Value</text>\n" +
                   $"<line stroke='#152060' stroke-width='1' x1='5' x2='240' y1='{height - 50}' y2='{height - 50}'/>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : #152060;' y='{height - 35}' x='10'>Prop02: Sample Property Value</text>\n" +
                   $"<line stroke='#152060' stroke-width='1' x1='5' x2='240' y1='{height - 30}' y2='{height - 30}'/>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : #152060;' y='{height - 15}' x='10'>Prop03: Sample Property Value</text>" +
                   $"</g>\n\n";
        }

        public static string WriteSubNet(int x, int y, int height, int width = 370, bool networkSecurityGroup = false,
            bool loadBalancer = false, bool availabilitySet = false)
        {
            string subNetName = "CoreNetwork";
            String subnetPrefix = "10.1.0.0/24";
            if (loadBalancer)
            {
                width += 100;
            }
            if (availabilitySet)
            {
                width += 50;
            }

            string result = $"<g transform='translate({x} {y})'>\n" +
                            $"<rect x='0' y='10' width='{width}' height='{height}' fill='#f6f9fc' stroke='#2e6ca4' stroke-width='2'></rect>\n" +
                            $"<rect x='0' y='-10' width='200' height='20' fill='#2e6ca4' stroke='#2e6ca4' stroke-width='2'></rect>\n" +
                            $"<text style='font-size: 11pt;font-family: consolas;fill : white;' y='5' x='5'>Subnet :  {subNetName}</text>\n" +
                            $"<text style='font-size: 11pt;font-family: consolas;fill : #152060;' y='5' x='210'>Prefix : {subnetPrefix}</text>\n";
            if (networkSecurityGroup)
            {
                result += $"<image height='50' width='50' x='0' y='15' href='icons/s_shield_bl.png'></image>\n";
            }
            result += $"</g>\n\n";
            return result;
        }

        public static string WriteAvailabilitySet(int x, int y, int height)
        {
            string availablilitySetName = "CoreServices";
            return $"<g transform='translate({x} {y})'>\n" +
                   $"<rect x='0' y='0' width='355' height='{height}' fill='#fff3f3' stroke='#ff8B8B' stroke-width='2'></rect>\n" +
                   $"<rect x='80' y='-10' width='275' height='20' fill='#ff8B8B' stroke='#ff8B8B' stroke-width='2'></rect>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : #152060;' y='5' x='90'>Availability Set : {availablilitySetName}</text>\n" +
                   $"</g>\n\n";
        }

        public static string WriteLoadBalancer(int x, int y)
        {
            string privateIPAddress = "192.168.10.20";
            return $"<g transform='translate({x} {y})'>\n" +
                   $"<image height='60' width='60' x='8' y='0' xlink:href='icons/s_Azure-load-balancer.png'></image>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : black;' y='74' x='0'>Plivate IP:</text>\n" +
                   $"<text style='font-size: 9pt;font-family: consolas;fill : black;' y='90' x='0'>{privateIPAddress}</text>\n" +
                   $"</g>\n\n";
        }

        public static string WriteNIC(int x, int y, bool networkSecurityGroup = false)
        {
            string publicIPAddress = "130.44.10.195";
            string privateIPAddress = "192.168.10.100";
            string result = $"<g transform='translate({x} {y})'>\n" +
                            $"<line stroke='#8B2930' stroke-width='2' x1='-25' x2='275' y1='15' y2='15'/>\n" +
                            $"<circle stroke='#8B2930' stroke-width='2' fill='#EB8990'  cx='-25' cy='15' r='4'/>\n" +
                            $"<circle stroke='#8B2930' stroke-width='2' fill='#EB8990'  cx='275' cy='15' r='4'/>\n" +
                            $"<rect x='0' y='0' width='45' height='30' fill='#ffffff' stroke='#8B2930' stroke-width='2'></rect>\n" +
                            $"<rect x='45' y='0' width='215' height='30' fill='#8B2930' stroke='#8B2930' stroke-width='2'></rect>\n" +
                            $"<text style='font-size: 9pt;font-family: consolas;fill : white;' y='12' x='50'>Public IP: {publicIPAddress}</text>\n" +
                            $"<text style='font-size: 9pt;font-family: consolas;fill : white;' y='24' x='50'>Plivate IP: {privateIPAddress}</text>\n" +
                            $"<image height='30' width='30' x='8' y='0' xlink:href='icons/s2_Network.png'></image>\n";
            if (networkSecurityGroup)
            {
                result += "<image height='30' width='30' x='230' y='0' xlink:href='icons/s_shield.png'></image>\n";
            }
            result += "</g>\n\n";
            return result;
        }

        public static string GetSampleHtml()
        {
            var result = new StringBuilder();
            result.Append(HTMLGenerator.WriteHeader());
            int Xbase = 50;
            int YBase = 50;

            // VNET --------------------------------------
            result.Append(HTMLGenerator.WriteVNet(Xbase - 25, YBase - 25, 1450, 1050));

            // Subnet 1 ---------------------------
            result.Append(HTMLGenerator.WriteSubNet(Xbase + 10, YBase + 10, 950, networkSecurityGroup: true, loadBalancer: true, availabilitySet: true));

            result.Append(HTMLGenerator.WriteLoadBalancer(Xbase + 50, YBase + 245));

            result.Append(HTMLGenerator.WriteAvailabilitySet(Xbase + 160, YBase + 50, 490));
            result.Append(HTMLGenerator.WriteVM(Xbase + 220, YBase + 100, 230));
            result.Append(HTMLGenerator.WriteNIC(Xbase + 230, YBase + 135, networkSecurityGroup: true));
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
            result.Append(HTMLGenerator.WriteNIC(Xbase + 630, YBase + 115, networkSecurityGroup: true));
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

            return result.ToString();
        }
    }
}