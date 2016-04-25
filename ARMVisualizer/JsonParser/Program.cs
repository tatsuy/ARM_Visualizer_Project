using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            StreamReader streamReader = new StreamReader(jsonPath, Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<RootObject>(json);
        }
    }
}
