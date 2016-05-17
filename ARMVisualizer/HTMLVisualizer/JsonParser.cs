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
            result.Parameters.AddRange(GetParametersbyJsonObject(jsonObject["parameters"].ToArray()));
            result.Variables.AddRange(GetVariablesbyJsonObject(jsonObject["variables"].ToArray()));
            result.Schema = ResolveName(jsonObject["$schema"]?.ToString(), result.Variables, result.Parameters);
            result.ContentVersion = ResolveName(jsonObject["contentVersion"]?.ToString(), result.Variables, result.Parameters);
            result.Resources.AddRange(GetResourcesbyJsonObject(jsonObject["resources"].ToArray(), result.Variables, result.Parameters));

            return result;
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

        public static List<Resource> GetResourcesbyJsonObject(JToken[] resources, List<Variable> variables, List<Parameter> parameters)
        {

            List<Resource> result = new List<Resource>();
            foreach (var resource in resources)
            {
                result.Add(new Resource());
                result.Last().ApiVersion = ResolveName(resource["apiVersion"]?.ToString(), variables, parameters);
                result.Last().Type = ResolveName(resource["type"]?.ToString(), variables, parameters);
                result.Last().Name = ResolveName(resource["name"]?.ToString(), variables, parameters);
                result.Last().Location = ResolveName(resource["location"]?.ToString(), variables, parameters);
                if (resource["properties"]?.ToString() != null)
                    result.Last().properties.AddRange(GetPropertiesbyJsonObject(resource["properties"]?.ToArray(), variables, parameters));

                if (resource["dependsOn"]?.ToString() != null)
                    result.Last().DependsOn = GetDependsOnbyJsonObject(resource["dependsOn"]?.ToArray(), variables, parameters);
            }
            return result;
        }

        public static List<Property> GetPropertiesbyJsonObject(JToken[] propertyArray, List<Variable> variables, List<Parameter> parameters)
        {
            List<Property> result = new List<Property>();
            foreach (var property in propertyArray)
            {
                result.Add(new Property());
                result.Last().Name = ((JProperty)property).Name;
                if (property.First().Type == JTokenType.String)
                    result.Last().Value = ResolveName(property.First().ToString(), variables, parameters);
                else if (property.First().Type == JTokenType.Array)
                {
                    if (property.First().ToString() != "[]" && property.First().First().Type == JTokenType.String)
                        result.Last().Value = ResolveName(property.First().First().ToString(), variables, parameters);
                    else if (result.Last().Name == "subnets")
                        result.Last().Subnets.AddRange(GetSubnetsbyJsonObject(property.First().ToArray(), variables, parameters));
                    else
                        result.Last().Properties.AddRange(GetPropertybyJsonObject(property.First().ToArray(), variables, parameters));
                }
                else if (property.First().Type == JTokenType.Object)
                {
                    result.Last().Properties.AddRange(GetPropertiesbyJsonObject(property.First().ToArray(), variables, parameters));
                }
            }
            return result;
        }

        public static List<Property> GetPropertybyJsonObject(JToken[] propertyArray, List<Variable> variables, List<Parameter> parameters)
        {

            List<Property> result = new List<Property>();
            foreach (var property in propertyArray)
            {
                result.Add(new Property());
                if (((JProperty)property.First()).Name == "name")
                {
                    result.Last().Name = ResolveName(property["name"]?.ToString(), variables, parameters);
                    if (property["properties"]?.ToString() != null)
                        result.Last().Properties.AddRange(GetPropertiesbyJsonObject(property["properties"]?.ToArray(), variables, parameters));
                }
                else if (((JProperty)property.First()).Name == "id")
                {
                    result.Last().Name = ((JProperty)property.First()).Name;
                    result.Last().Value = ResolveName(((JProperty)(property.Last())).Value.ToString(), variables, parameters);
                }

            }
            return result;
        }

        public static List<Subnet> GetSubnetsbyJsonObject(JToken[] propertyArray, List<Variable> variables, List<Parameter> parameters)
        {

            List<Subnet> result = new List<Subnet>();
            foreach (var property in propertyArray)
            {
                result.Add(new Subnet());
                result.Last().Name = ResolveName(property["name"]?.ToString(), variables, parameters);
                if (property["properties"]?.ToString() != null)
                {
                    result.Last().AddressPrefix = ResolveName(property["properties"]?["addressPrefix"]?.ToString(), variables, parameters);
                    if (property["networkSecurityGroup"]?.ToString() != null)
                    {
                        result.Last().NetworkSecurityGroup = property["networkSecurityGroup"]?.Last().ToString();
                    }
                }
            }
            return result;
        }

        public static List<string> GetDependsOnbyJsonObject(JToken[] dependsOns, List<Variable> variables, List<Parameter> parameters)
        {
            var result = new List<string>();
            foreach (var dependsOn in dependsOns)
            {
                result.Add(ResolveName(dependsOn.ToString(), variables, parameters));
            }
            return result;
        }

        public static void OutputJsonFile(RootObject root)
        {

            string outputJson = JsonConvert.SerializeObject(root);
            File.WriteAllText("Output.json", outputJson);
        }

        private static string ResolveName(string json, List<Variable> variables, List<Parameter> parameters)
        {
            if (json == null) return null;
            var result = json;
            if(result.Length > 2 && result[0] == '[' && result[result.Length - 1] == ']')
            {
                result = result.TrimStart('[').TrimEnd(']');
                result = ResolveVariables(result, variables, "variables");
                result = ResolveParameters(result, parameters, "parameters");
                result = ResolveConcat(result);
                result = ResolveResourceId(result);
            }
            return result;
        }

        private static string ResolveVariables(string json, List<Variable> values, string name)
        {
            var currentPosition = 0;
            var startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            while (startIndex != -1)
            {
                var endIndex = json.IndexOf("')", startIndex, StringComparison.OrdinalIgnoreCase);
                // extract of "parameters('XXXXX')"
                var parameter = json.Substring(startIndex, endIndex + "')".Length - startIndex);

                var replaceStringStartIndex = startIndex + (name + "('").Length;
                // extract of parameter XXXXX
                var replaceString = json.Substring(replaceStringStartIndex, endIndex - replaceStringStartIndex);

                if (values.Exists(_ => _.Name == replaceString))
                {
                    var replacedString = values.Find(_ => _.Name == replaceString).Value;
                    json = json.Replace(parameter, replacedString);
                    currentPosition = json.IndexOf(replacedString);
                }
                else
                    currentPosition = json.IndexOf(replaceString);
                startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            }

            return json;
        }

        private static string ResolveParameters(string json, List<Parameter> values, string name)
        {
            var currentPosition = 0;
            var startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            while (startIndex != -1)
            {
                var endIndex = json.IndexOf("')", startIndex, StringComparison.OrdinalIgnoreCase);
                // extract of "parameters('XXXXX')"
                var parameter = json.Substring(startIndex, endIndex + "')".Length - startIndex);

                var replaceStringStartIndex = startIndex + (name + "('").Length;
                // extract of parameter XXXXX
                var replaceString = json.Substring(replaceStringStartIndex, endIndex - replaceStringStartIndex);

                if (values.Exists(_ => _.Name == replaceString))
                {
                    var replacedString = values.Find(_ => _.Name == replaceString).Value;
                    json = json.Replace(parameter, replacedString);
                    currentPosition = json.IndexOf(replacedString);
                }
                else
                    currentPosition = json.IndexOf(replaceString);
                startIndex = json.IndexOf(name, currentPosition, StringComparison.OrdinalIgnoreCase);
            }

            return json;
        }

        private static string ResolveConcat(string json)
        {
            bool quotationCheck = false;
            var result = string.Empty;
            string moji = string.Empty;
            var currentPosition = 0;
            var startIndex = json.IndexOf("concat", currentPosition, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1) return json;
            var position = startIndex + "concat(".Length;
            while (startIndex != -1 && position < json.Length)
            {
                switch (json[position])
                {
                    case ' ':
                        break;
                    case ')':
                        result += moji;
                        result += json.Substring(position + 1);
                        moji = string.Empty;
                        quotationCheck = false;
                        startIndex = -1;
                        break;
                    case ',':
                        if (quotationCheck)
                            goto default;
                        else
                        {
                            result += moji;
                            moji = string.Empty;
                        }
                        break;
                    case '\'':
                        if (quotationCheck)
                        {
                            result += moji;
                            moji = string.Empty;
                            quotationCheck = false;
                        }
                        else
                        {
                            if (moji == string.Empty)
                                quotationCheck = true;
                            else
                                goto default;
                        }
                        break;
                    default:
                        moji += json[position];
                        break;
                }
                position++;
                if (moji == "concat")
                {
                    var replaceString = json.Substring(position - "concat".Length);
                    json = json.Replace(replaceString, ResolveConcat(replaceString));
                    position -= "concat".Length;
                    moji = string.Empty;
                }
                if (moji == "resourceId")
                {
                    var replaceString = json.Substring(position - "resourceId".Length);
                    json = json.Replace(replaceString, ResolveResourceId(replaceString));
                    position -= "resourceId".Length;
                    moji = string.Empty;
                }
            }

            return result;
        }

        private static string ResolveResourceId(string json)
        {
            bool quotationCheck = false;
            var result = "/subscriptions/";
            var parameterCount = 0;
            string moji = string.Empty;
            var currentPosition = 0;
            var startIndex = json.IndexOf("resourceId", currentPosition, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1) return json;
            var position = startIndex + "resourceId(".Length;
            while (startIndex != -1 && position < json.Length)
            {
                switch (json[position])
                {
                    case ' ':
                        break;
                    case ')':
                        result += moji;
                        result += json.Substring(position + 1);
                        moji = string.Empty;
                        quotationCheck = false;
                        startIndex = -1;
                        break;
                    case ',':
                        if (quotationCheck)
                            goto default;
                        else
                        {
                            result += moji;
                            if (parameterCount == 0)
                                result += "/resourceGroups/";
                            else
                                result += "/";
                            moji = string.Empty;
                        }
                        break;
                    case '\'':
                        if (quotationCheck)
                        {
                            result += moji;
                            if (parameterCount == 0)
                                result += "/resourceGroups/";
                            moji = string.Empty;
                            quotationCheck = false;
                        }
                        else
                        {
                            if (moji == string.Empty)
                                quotationCheck = true;
                            else
                                goto default;
                        }
                        break;
                    default:
                        moji += json[position];
                        break;
                }
                position++;
                if (moji == "concat")
                {
                    var replaceString = json.Substring(position - "concat".Length);
                    json = json.Replace(replaceString, ResolveConcat(replaceString));
                    position -= "concat".Length;
                    moji = string.Empty;
                }
                if (moji == "resourceId")
                {
                    var replaceString = json.Substring(position - "resourceId".Length);
                    json = json.Replace(replaceString, ResolveResourceId(replaceString));
                    position -= "resourceId".Length;
                    moji = string.Empty;
                }
            }

            return result;
        }
    }
}
