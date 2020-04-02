using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Model
{
    public class Symbol
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool Parameter { get; set; }
        public bool Shared { get; set; }

        public Symbol(string name, Type type, bool parameter = false, bool shared = false)
        {
            if (parameter && shared)
                throw new ArgumentException("Parameter and Shared cannot be both true.");

            Name = name;
            Type = type;
            Parameter = parameter;
            Shared = shared;
        }

        public override string ToString()
        {
            return $"{Name} Is {Type.Name}";
        }
    }
}
