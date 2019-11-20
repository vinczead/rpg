using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.SymbolTable
{
    public class TypeSystem
    {
        Dictionary<string, Type> types;

        public Type BOOLEAN { get; private set; }
        public Type NUMBER { get; private set; }
        public Type STRING { get; private set; }
        public Type ERROR { get; private set; }
        public Type NULL { get; private set; }


        public TypeSystem()
        {
            types = new Dictionary<string, Type>();
            InitBuiltInTypes();
        }

        private void InitBuiltInTypes()
        {
            ERROR = new Type("ErrorType");
            STRING = new Type("String");
            NUMBER = new Type("Number");
            BOOLEAN = new Type("Boolean");
            NULL = new Type("Null");

            ERROR.Parents.Add(STRING);
            ERROR.Parents.Add(NUMBER);
            ERROR.Parents.Add(BOOLEAN);

            types[ERROR.Name] = ERROR;
            types[STRING.Name] = STRING;
            types[NUMBER.Name] = NUMBER;
            types[BOOLEAN.Name] = BOOLEAN;
        }

        public Type this[string name]
        {
            get
            {
                if (types.ContainsKey(name))
                    return types[name];
                return null;
            }
        }
    }
}
