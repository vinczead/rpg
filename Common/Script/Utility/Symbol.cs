using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Script.Utility
{
    public class Symbol
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }

        public object RealValue
        {
            get
            {
                if (Type == TypeSystem.Instance["Number"])
                    return double.Parse(Value);

                if (Type == TypeSystem.Instance["Boolean"])
                    return bool.Parse(Value);

                if (Type.InheritsFrom(TypeSystem.Instance["Thing"]))
                    return new Thing() { Id = Value };

                if (Type.InheritsFrom(TypeSystem.Instance["ThingInstance"]))
                    return new ThingInstance() { Id = Value };

                if (Type == TypeSystem.Instance["Tile"])
                    return new Tile() { Id = Value };


                if (Type == TypeSystem.Instance["Model"])
                    return new SpriteModel() { Id = Value };


                if (Type == TypeSystem.Instance["Texture"])
                    return new Texture() { Id = Value };

                if (Type == TypeSystem.Instance["Region"])
                    return new Region() { Id = Value };

                return Value;
            }
        }

        public Symbol(Symbol symbol)
        {
            Name = symbol.Name;
            Type = symbol.Type;
            Value = symbol.Value;
        }

        public Symbol(string name, Type type, string value = "")
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} : {Type.Name}" + (Value == "" ? "" : (" = " + Value));
        }
    }
}
