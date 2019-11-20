using System.Collections.Generic;
using System.Text;

namespace GameScript.SymbolTable
{
    public class Env
    {
        public string Name { get; set; }
        public Env Previous { get; private set; }
        private Dictionary<string, Symbol> Table { get; set; } = new Dictionary<string, Symbol>();

        public Env(Env previous = null, string name = "Default Scope")
        {
            Previous = previous;
            Name = name;
        }

        public Symbol this[string name]
        {
            get
            {
                return Table.ContainsKey(name) ? Table[name] : Previous?[name];
            }
            set
            {
                if (Table.ContainsKey(name))
                    throw new NameAlreadyDefinedException($"'{name}' is already defined.");
                Table[name] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();
            if (Previous != null)
                bld.Append(Previous.ToString());
            bld.AppendLine($"-----{Name}-----");
            foreach (var symbol in Table.Values)
                bld.AppendLine(symbol.ToString());
            return bld.ToString();
        }
    }
}
