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
        public bool Readonly { get; set; }

        public Symbol(string name, Type type, bool @readonly=false)
        {
            Name = name;
            Type = type;
            Readonly = @readonly;
        }

        public override string ToString()
        {
            return $"{Name} Is {Type.Name}";
        }
    }
}
