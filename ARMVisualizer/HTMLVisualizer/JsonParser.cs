using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HTMLVisualizer
{
    public class JsonParser
    {
        public static RootObject ConvertJson(string jsonPath)
        {
            var jsonObject = ConvertJsonObject(jsonPath);
            RootObject result = new RootObject();
            result.Schema = jsonObject["$schema"]?.ToString();
            result.ContentVersion = jsonObject["contentVersion"]?.ToString();
            result.Parameters.AddRange(GetParametersbyJsonObject(jsonObject["parameters"].ToArray()));
            result.Variables.AddRange(GetVariablesbyJsonObject(jsonObject["variables"].ToArray()));
            result.Resources.AddRange(GetResourcesbyJsonObject(jsonObject["resources"].ToArray()));

            return null;
        }

        public static JObject ConvertJsonObject(string jsonPath)
        {

            string json = File.ReadAllText(jsonPath);
            return JObject.Parse(json);
        }

        public static List<Parameter> GetParametersbyJsonObject(JToken[] parameters)
        {
            List<Parameter> result = new List<Parameter>();
            foreach (var parameter in parameters)
            {
                result.Add(new Parameter());
                result.Last().Name = ((JProperty)parameter).Name;
                result.Last().Value = ((JProperty)((JContainer)(parameter.First())).First).Value.ToString();
            }
            return result;
        }

        public static List<Variable> GetVariablesbyJsonObject(JToken[] variables)
        {
            List<Variable> result = new List<Variable>();
            foreach (var variable in variables)
            {
                result.Add(new Variable());
                result.Last().Name = ((JProperty)variable).Name;
                result.Last().Value = variable.First().ToString();
            }
            return result;
        }

        public static List<Resource> GetResourcesbyJsonObject(JToken[] resources)
        {

            List<Resource> result = new List<Resource>();
            foreach (var resource in resources)
            {
                result.Add(new Resource());
                result.Last().ApiVersion = resource["apiVersion"]?.ToString();
                result.Last().Type = resource["type"]?.ToString();
                result.Last().Name = resource["name"]?.ToString();
                result.Last().Location = resource["location"]?.ToString();
                if (resource["properties"]?.ToString() != null)
                    result.Last().properties.AddRange(GetPropertiesbyJsonObject(resource["properties"].ToArray()));

                if (resource["dependsOn"]?.ToString() != null)
                    result.Last().DependsOn = GetDependsOnbyJsonObject(resource["dependsOn"].ToArray());
            }
            return result;
        }

        public static List<Property> GetPropertiesbyJsonObject(JToken[] propertyArray)
        {

            List<Property> result = new List<Property>();
            foreach (var property in propertyArray)
            {
                result.Add(new Property());
                result.Last().Name = ((JProperty)property).Name;
                if(property.First().Type == JTokenType.String)
                    result.Last().Value = property.First().ToString();
                else if (property.First().Type == JTokenType.Array)
                    result.Last().properties.AddRange(GetPropertiesbyJsonObject(property["properties"].ToArray()));
            }
            return result;
        }

        public static List<string> GetDependsOnbyJsonObject(JToken[] dependsOns)
        {
            var result = new List<string>();
            foreach (var dependsOn in dependsOns)
            {
                result.Add(dependsOn.ToString());
            }
            return result;
        }

        public static void OutputJsonFile(RootObject root)
        {

            string outputJson = JsonConvert.SerializeObject(root);
            File.WriteAllText("Output.json", outputJson);
        }

        public static string ResolveParameters(string json, List<Parameter> parameters)
        {
            return ResolveValue(json, parameters, "parameters");
        }

        private static string ResolveValue(string json, List<Parameter> parameters, string name)
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
