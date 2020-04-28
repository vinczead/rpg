using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public class Symbol
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }

        public Symbol(Symbol symbol)
        {
            Name = symbol.Name;
            Type = symbol.Type;
            Value = symbol.Value;
        }

        public Symbol(string name, Type type, string value = "")
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} : {Type.Name}";
        }
    }
}
