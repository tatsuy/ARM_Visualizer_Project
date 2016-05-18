using System;
using System.IO; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.InteropServices;
using ARMJsonTest;

namespace HTMLVisualizer
{


    public partial class Form1 : Form
    {
        ChromiumWebBrowser m_chromeBrowser = null;
        JavaScriptInteractionObj m_jsInteractionObj = null;

        public List<ARMResources> BuildSampleResource()
        {
            List<ARMResources> r = new List<ARMResources> ();

            r.Add(new ARMResources("VNET", (int)ARMResourceType.ARM_Vnet, null));
            r.Add(new ARMResources("subnet1", (int)ARMResourceType.ARM_Subnet, "VNET"));
            r.Add(new ARMResources("subnet2", (int)ARMResourceType.ARM_Subnet, "VNET"));
            r.Add(new ARMResources("myavlsetf", (int)ARMResourceType.ARM_AvailabilitySet, "subnet1"));
            r.Add(new ARMResources("myavlsetb", (int)ARMResourceType.ARM_AvailabilitySet, "subnet2"));

            r.Add(new ARMResources("FLB", (int)ARMResourceType.ARM_LoadBalancer, "subnet1"));
            r.Add(new ARMResources("ILB", (int)ARMResourceType.ARM_LoadBalancer, "subnet2"));

            r.Add(new ARMResources("VM1", (int)ARMResourceType.ARM_VirtualMachine, "myavlsetf"));
            r.Add(new ARMResources("VM2", (int)ARMResourceType.ARM_VirtualMachine, "myavlsetf"));
            r.Add(new ARMResources("VMB1", (int)ARMResourceType.ARM_VirtualMachine, "myavlsetb"));
            r.Add(new ARMResources("VMB2", (int)ARMResourceType.ARM_VirtualMachine, "myavlsetb"));

            return r;
        }

        public List<ARMResources> BuildARMResource(List<Resource> resources)
        {
            List<ARMResources> r = new List<ARMResources>();
            foreach (var resource in resources)
            {
                var types = resource.Type.Split('/');
                switch (types.Last())
                {
                    case "virtualMachines":
                        var parent = string.Empty;
                        if (resource.properties.Exists(_ => _.Name == "availabilitySet"))
                        {
                            parent = resource.properties.Find(_ => _.Name == "availabilitySet").Properties.First().Value.Split('/').Last();

                            var nicName = resource.properties.Find(_ => _.Name == "networkProfile").Properties.Find(_ => _.Name == "networkInterfaces").Properties.First().Value.Split('/').Last();
                            // TODO: ipConfigurations は複数いるが、今回は一つとみなして処理
                            var subnet = resources.Find(_ => _.Name == nicName).properties.Find(_ => _.Name == "ipConfigurations").Properties.First().Properties.Find(_ => _.Name == "subnet").Properties.First().Value.Split('/').Last();
                            var addressPrefix = resources.Find(_ => _.Type.Split('/').Last() == "virtualNetworks").properties.Find(p => p.Name == "subnets").Subnets.Find(s => s.Name == subnet).AddressPrefix;

                            // AvailabilitySet のリソースを追加
                            if (!r.Exists(_ => _.resname == parent))
                                r.Add(new ARMResources(parent, (int)ARMResourceType.ARM_AvailabilitySet, subnet));

                            // Subnet のリソースを追加
                            // TODO: 最終的には virtualNetworks は複数あるものとして処理を変更する
                            if (!r.Exists(_ => _.resname == subnet))
                                r.Add(new ARMResources(subnet, (int)ARMResourceType.ARM_Subnet, resources.Find(_ => _.Type.Split('/').Last() == "virtualNetworks").Name, addressPrefix));

                        }
                        else
                        {
                            var nicName = resource.properties.Find(_ => _.Name == "networkProfile").Properties.First().Value.Split('/').Last();
                            parent = resources.Find(_ => _.Name == nicName).properties.Find(_ => _.Name == "ipConfigurations").Properties.Find(_ => _.Name == "subnet").Value.Split('/').Last();
                            var addressPrefix = resources.Find(_ => _.Type.Split('/').Last() == "virtualNetworks").properties.Find(p => p.Name == "subnets").Subnets.Find(s => s.Name == parent).AddressPrefix;

                            // Subnet のリソースを追加
                            // TODO: 最終的には virtualNetworks は複数あるものとして処理を変更する
                            if (!r.Exists(_ => _.resname == parent))
                                r.Add(new ARMResources(parent, (int)ARMResourceType.ARM_Subnet, resources.Find(_ => _.Type.Split('/').Last() == "virtualNetworks").Name, addressPrefix));
                        }
                        // VirtualMachine のリソースを追加
                        r.Add(new ARMResources(resource.Name, (int)ARMResourceType.ARM_VirtualMachine, parent, resource.properties.Find(_ => _.Name == "hardwareProfile").Properties.First().Value));
                        break;
                    case "loadBalancers":
                        // LoadBalancer のリソースを追加
                        var loadBalancerFrontEnd = resource.properties.Find(_ => _.Name == "frontendIPConfigurations").Properties.First();
                        if (loadBalancerFrontEnd.Properties.Find(_ => _.Name == "subnet") != null)
                        {
                            parent = resource.properties.Find(_ => _.Name == "frontendIPConfigurations").Properties.First().Properties.Find(_ => _.Name == "subnet").Properties.First().Value.Split('/').Last();
                            r.Add(new ARMResources(resource.Name, (int)ARMResourceType.ARM_LoadBalancer, parent, loadBalancerFrontEnd.Properties.Find(_ => _.Name == "privateIPAddress").Value));
                        }
                        break;
                    case "virtualNetworks":
                        r.Add(new ARMResources(resource.Name, (int)ARMResourceType.ARM_Vnet, null));

                        var subnets = resource.properties.Find(_ => _.Name == "subnets");

                        // TODO: virtualNetworks がリソースの最後に来ることを前提とした処理のため、後で変更が必要
                        foreach (var subnet in subnets.Subnets)
                        {
                            if (subnet.NetworkSecurityGroup == null) continue;
                            foreach (var res in r.FindAll(_ => _.resname == subnet.Name))
                            {
                                res.nsgpresent = true;
                            }
                            
                        }
                        break;
                    default:
                        break;
                }
            }

            return r;
        }

        public List<ARMResources> GetProperty(List<Property> properties, string parent)
        {
            List<ARMResources> r = new List<ARMResources>();
            foreach (var property in properties)
            {
                if (property.Name == "subnets")
                {

                }
                if (property.Name == "subnet")
                {

                }
                if (property.Properties != null && property.Properties.Count > 0)
                    r.AddRange(GetProperty(property.Properties, parent));
            }
            return r;
        }

        public void CaluculatePosition(List<ARMResources> r)
        {
            for (int i = 0; i < 4; i++)
            { 
                r.OrderBy(_ => _.level);
                foreach (var item in r)
                {
                    if (item.resparent != null)
                    {
                        foreach (var item2 in r)
                        {
                            if (item.resparent == item2.resname)
                            {
                                if (item2.level != -1)
                                {
                                    item.level = item2.level + 1;
                                }
                            }
                        }
                    }
                }
            }
            r.OrderBy(_ => _.restype);
            r.OrderBy(_ => _.level);

            foreach (var item in r)
            {
                if (item.resparent != null)
                {
                    ARMResources t = r.Find(_ => _.resname.Equals(item.resparent));
                    if (t == null)
                    {
                        continue;
                    }
                    t.reschild.Add(item.resname);
                }
            }

            int current;
            int numSubnet = 0 ;
            int numVM = 0;

            /// Vnet Loop
            foreach (var item in r)
            {
                current = item.level;
                
                if (item.restype == (int)ARMResourceType.ARM_Vnet)
                {
                    item.x = 30;
                    item.y = 30;
                        
                    item.width = 400 * (r.Max(_ => _.level) + 1);
                    item.height = 1200;

                    /// Subnet Loop
                    foreach (var item2 in r)
                    {
                        if (item2.restype == (int)ARMResourceType.ARM_Subnet)
                        {
                            foreach (var opitem in r)
                            {
                                if ((opitem.restype == (int)ARMResourceType.ARM_AvailabilitySet) && 
                                    opitem.resparent == item2.resname)
                                {
                                    item2.avsetpresent = true;
                                }
                                if ((opitem.restype == (int)ARMResourceType.ARM_LoadBalancer) &&
                                    opitem.resparent == item2.resname)
                                {
                                    item2.lbpresent = true;

                                    opitem.x = (numSubnet * 550) + 100;
                                    opitem.y = 100;
                                }
                                if ((opitem.restype == (int)ARMResourceType.ARM_NetworkSecurityGroup) &&
                                    opitem.resparent == item2.resname)
                                {
                                    item2.nsgpresent = true;
                                }
                            }

                            item2.x = (numSubnet * 550) + 100;
                            item2.y = 100;
                            numSubnet++;

                            numVM = 0;

                            /// AVset Loop
                            foreach (var item3 in r)
                            {
                                if (item3.restype == (int)ARMResourceType.ARM_AvailabilitySet)
                                {
                                    if (item3.resparent == item2.resname)
                                    {
                                        item3.x = item2.x + 20;
                                        item3.y = item2.y + 50;

                                        /// VM loop
                                        foreach (var item4 in r)
                                        {
                                            if ((item4.resparent == item3.resname) &&
                                                (item4.restype == (int)ARMResourceType.ARM_VirtualMachine))
                                            {
                                                item4.x = item3.x + 80;
                                                item4.y = item3.y + 50 + (numVM * 100);
                                                item4.height = 50;
                                                numVM++;
                                            }
                                        }

                                        item3.height = (numVM * 100) + 50;
                                    }
                                }

                                if ((item3.restype == (int)ARMResourceType.ARM_VirtualMachine) &&
                                    item3.resparent == item2.resname)
                                {
                                    item3.x = item2.x + 80;
                                    item3.y = item2.y + 50 + (numVM * 100);
                                    item3.height = 50;
                                    numVM++;
                                }
                            }
                            item2.height = numVM * 60 + 220;
                        }
                    }                    
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            string[] cmds = System.Environment.GetCommandLineArgs();
            string url = Directory.GetCurrentDirectory() + "\\html\\test.html";
            foreach (string cmd in cmds)
            {
                if (cmd.Contains("htm"))
                {
                    url = cmd;
                    break;
                } 
            }*/

            var result = JsonParser.ConvertJson("html\\template.json");
            List<ARMResources> res = BuildARMResource(result.Resources);
            CaluculatePosition(res);

            List<ARMResources> a = new List<ARMResources>();
            a = res.OrderBy(_ => _.level).ToList();

            File.WriteAllText("test.html", HTMLGenerator.GetHtmlFromARMResources(a));

            // File.WriteAllText("test.html", HTMLGenerator.GetSampleHtml());
            string url = Path.Combine(Environment.CurrentDirectory, "test.html");

            m_chromeBrowser = new ChromiumWebBrowser(url);

            panel1.Controls.Add(m_chromeBrowser);

            ChromeDevToolsSystemMenu.CreateSysMenu(this);

            m_jsInteractionObj = new JavaScriptInteractionObj(result.Resources);
            m_jsInteractionObj.SetChromeBrowser(m_chromeBrowser);

            // Register the JavaScriptInteractionObj class with JS
            m_chromeBrowser.RegisterJsObject("winformObj", m_jsInteractionObj);

            ChromeDevToolsSystemMenu.CreateSysMenu(this);

        }
    }

    static class ChromeDevToolsSystemMenu
    {
        // P/Invoke constants
        public static int WM_SYSCOMMAND = 0x112;
        public static int MF_STRING = 0x0;
        public static int MF_SEPARATOR = 0x800;

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // ID for the Chrome dev tools item on the system menu
        public static int SYSMENU_CHROME_DEV_TOOLS = 0x1;

        public static void CreateSysMenu(Form frm)
        {
            // in your form override the OnHandleCreated function and call this method e.g:
            // protected override void OnHandleCreated(EventArgs e)
            // {
            //     ChromeDevToolsSystemMenu.CreateSysMenu(frm,e);
            // }

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(frm.Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_CHROME_DEV_TOOLS, "&Chrome Dev Tools");
        }
    }
}
