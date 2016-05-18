using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARMJsonTest
{
    public class Resource
    {
        public Resource(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = birthDate;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SkillLevel { get; set; }
    }

    public class JavaScriptInteractionObj
    {
        public List<HTMLVisualizer.Resource> _resources = new List<HTMLVisualizer.Resource>();

        [JavascriptIgnore]
        public ChromiumWebBrowser m_chromeBrowser { get; set; }

        public JavaScriptInteractionObj(List<HTMLVisualizer.Resource> resources)
        {
            _resources = resources;
        }

        [JavascriptIgnore]
        public void SetChromeBrowser(ChromiumWebBrowser b)
        {
            m_chromeBrowser = b;
        }

        public void SomeFunction()
        {
            var message = string.Empty;
            var nsg = _resources.Find(_ => _.Type.Split('/').Last() == "networkSecurityGroups");
            if (nsg != null && nsg.properties.Find(_ => _.Name == "securityRules") != null)
            {
                var securityRules = nsg.properties.Find(_ => _.Name == "securityRules").Properties;
                if (securityRules != null && securityRules.Count > 0)
                {
                    var count = 1;
                    foreach (var securityRule in securityRules)
                    {
                        message += $"ルール {count}: {securityRule.Name}{Environment.NewLine}";
                        foreach (var rule in securityRule.Properties)
                        {
                            message += $"{rule.Name} ({rule.Value}){Environment.NewLine}";
                        }
                        message += Environment.NewLine;
                        count++;
                    }
                }
                MessageBox.Show(message);
            }
            else
                MessageBox.Show("NetworkSecurityGroup にルールが存在しません。");
        }

        public string GetPerson()
        {
            var p1 = new Resource("Bruce", "Banner", DateTime.Now);

            string json = JsonConvert.SerializeObject(p1);

            return json;
        }

        public string ErrorFunction()
        {
            return null;
        }

        public string GetListOfPeople()
        {
            List<Resource> peopleList = new List<Resource>();

            peopleList.Add(new Resource("Scooby", "Doo", DateTime.Now));
            peopleList.Add(new Resource("Buggs", "Bunny", DateTime.Now));
            peopleList.Add(new Resource("Daffy", "Duck", DateTime.Now));
            peopleList.Add(new Resource("Fred", "Flinstone", DateTime.Now));
            peopleList.Add(new Resource("Iron", "Man", DateTime.Now));

            string json = JsonConvert.SerializeObject(peopleList);

            return json;
        }

        public void ExecJSFromWinForms()
        {
            var script = "document.body.style.backgroundColor = 'red';";

            m_chromeBrowser.ExecuteScriptAsync(script);
        }

        public void TestJSCallback(IJavascriptCallback javascriptCallback)
        {
            using (javascriptCallback)
            {
                javascriptCallback.ExecuteAsync("Hello from winforms and C# land!");
            }
        }
    }
}