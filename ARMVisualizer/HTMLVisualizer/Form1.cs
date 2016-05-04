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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] cmds = System.Environment.GetCommandLineArgs();
            string url = Directory.GetCurrentDirectory() + "\\html\\test.html";
            foreach (string cmd in cmds)
            {
                if (cmd.Contains("htm"))
                {
                    url = cmd;
                    break;
                } 
            }

            m_chromeBrowser = new ChromiumWebBrowser(url);

            panel1.Controls.Add(m_chromeBrowser);

            ChromeDevToolsSystemMenu.CreateSysMenu(this);

            m_jsInteractionObj = new JavaScriptInteractionObj();
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
