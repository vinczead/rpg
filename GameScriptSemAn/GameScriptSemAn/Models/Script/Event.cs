using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public class Event
    {
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
