using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;

namespace JsonParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var root = ConvertJson(args[0]);
        }

        public static RootObject ConvertJson(string jsonPath)
        {
            var jsonObject = ConvertJsonObject(jsonPath);


            return null;
        }

        public static JObject ConvertJsonObject(string jsonPath)
        {

            string json = File.ReadAllText(jsonPath);
            return JObject.Parse(json);
        }

        public static void OutputJsonFile(RootObject root)
        {
            
            string outputJson = JsonConvert.SerializeObject(root);
            File.WriteAllText("Output.json", outputJson);
        }

        public static string ResolveParameters(string json, Parameters parameters)
        {
            return ResolveValue(json, parameters, "parameters");
        }

        private static string ResolveValue(string json, Parameters parameters, string name)
        {
            var currentPosition = 0;
            var startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            while (startIndex != -1)
            {
                var endIndex = json.IndexOf("')", startIndex, StringComparison.OrdinalIgnoreCase);
                // extract of "parameters('XXXXX')"
                var parameter = json.Substring(startIndex, endIndex + "')".Length - startIndex);

                var replaceStringStartIndex = (name + "('").Length;
                var replaceStringEndIndex = parameter.Length - "')".Length;
                // extract of parameter XXXXX
                var replaceString = json.Substring(replaceStringStartIndex, replaceStringEndIndex - replaceStringStartIndex + 1);
                var t = parameters.GetType();
                object o = Activator.CreateInstance(t);
                json = json.Replace(parameter, (string)t.GetField(replaceString).GetValue(o));
                currentPosition = startIndex;
                startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            }

            return json;
        }
    }
}
