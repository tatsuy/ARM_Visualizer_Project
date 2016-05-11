using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace JsonParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var root = ConvertJson(args[0]);
        }

        static public RootObject ConvertJson(string jsonPath)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            var json = File.ReadAllText(jsonPath);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (RootObject)ser.ReadObject(memoryStream);
            //string json = File.ReadAllText(jsonPath);
            //return JsonConvert.DeserializeObject<RootObject>(json);
        }

        static public void OutputJsonFile(RootObject root)
        {
            
            string outputJSON = JsonConvert.SerializeObject(root);
            File.WriteAllText("Output.json", outputJSON);
        }
    }
}
