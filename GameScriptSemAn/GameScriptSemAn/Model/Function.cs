using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScriptSemAn.Model
{
    public class Function
    {
        public string SubjectType { get; set; }
        public string Name { get; set; }
        public string ReturnValueType { get; set; }
        public string Description { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
