using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader(args[0], Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<RootObject>(json);
        }
    }
}
