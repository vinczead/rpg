using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg
{
    public class Quest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string,string> Variables { get; set; }
    }
}