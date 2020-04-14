﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameScript.Model
{
    public class Env
    {
        public string Name { get; set; }
        public Env Previous { get; private set; }
        public Type BaseClassType { get; set; }

        private Dictionary<string, Symbol> table = new Dictionary<string, Symbol>();

        public Env(Env previous = null, string name = "Default Scope")
        {
            Previous = previous;
            Name = name;
        }

        public Symbol this[string name]
        {
            get
            {
                if (name == null)
                    return null;
                return table.ContainsKey(name) ? table[name] : Previous?[name];
            }
            set
            {
                if (table.ContainsKey(name))
                    throw new ArgumentException($"'{name}' is already defined.", "name");
                table[name] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();
            if (Previous != null)
                bld.Append(Previous.ToString());
            bld.AppendLine($"-----{Name}-----");
            foreach (var symbol in table.Values)
                bld.AppendLine(symbol.ToString());
            return bld.ToString();
        }
    }
}
