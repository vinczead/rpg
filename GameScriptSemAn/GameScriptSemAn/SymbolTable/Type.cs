using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScriptSemAn.SymbolTable
{
    public class Type
    {
        public string Name { get; set; }
        public HashSet<Type> Parents { get; set; }

        public Type(string name)
        {
            Name = name;
            Parents = new HashSet<Type>();
        }

        public bool InheritsFrom(Type t)
        {
            return this == t || this.Parents.Any(p => p.InheritsFrom(t));
        }
    }
}
