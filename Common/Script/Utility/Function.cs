using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Script.Utility
{
    public class Function
    {
        public string Name { get; set; }
        public Type ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Parameter
    {
        public Type Type { get; set; }
        public string Name { get; set; }
    }
}
