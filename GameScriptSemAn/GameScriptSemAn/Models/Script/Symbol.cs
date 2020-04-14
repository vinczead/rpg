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

        public Symbol(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name} Is {Type.Name}";
        }
    }
}
