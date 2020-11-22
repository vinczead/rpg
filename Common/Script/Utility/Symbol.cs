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
                {
                    if (double.TryParse(Value, out var realValue))
                        return realValue;
                    else return 0.0;
                }

                if (Type == TypeSystem.Instance["Boolean"])
                {
                    if (bool.TryParse(Value, out var realValue))
                        return realValue;
                    else return false;
                }

                if (Type == TypeSystem.Instance["Thing"])
                {
                    if (Value == "")
                        return null;
                    else return new Thing() { Id = Value };
                }

                if (Type == TypeSystem.Instance["ThingInstance"])
                {
                    if (Value == "")
                        return null;
                    else return new ThingInstance() { Id = Value };
                }

                if (Type == TypeSystem.Instance["Tile"])
                {
                    if (Value == "")
                        return null;
                    else return new Tile() { Id = Value };
                }

                if (Type == TypeSystem.Instance["SpriteModel"])
                {
                    if (Value == "")
                        return null;
                    else return new SpriteModel() { Id = Value };
                }

                if (Type == TypeSystem.Instance["Region"])
                {
                    if (Value == "")
                        return null;
                    else return new Region() { Id = Value };
                }

                if (Type == TypeSystem.Instance["Texture"])
                {
                    if (Value == "")
                        return null;
                    else return new Texture() { Id = Value };
                }

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
