using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLVisualizer
{
    public class RootObject
    {
        public string Schema { get; set; }
        public string ContentVersion { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public List<Variable> Variables { get; set; } = new List<Variable>();
        public List<Resource> Resources { get; set; } = new List<Resource>();
    }

    public class Resource
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ApiVersion { get; set; }
        public string Location { get; set; }
        public List<Property> properties { get; set; } = new List<Property>();
        public List<string> DependsOn { get; set; } = new List<string>();
    }

    public class Property
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<Property> properties { get; set; } = new List<Property>();
    }
    
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Variable
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
