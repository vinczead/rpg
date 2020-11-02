using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Script.Utility
{
    public sealed class FunctionManager
    {
        public static FunctionManager Instance { get; } = new FunctionManager();

        private Dictionary<string, Function> functions;

        private FunctionManager()
        {
            functions = new Dictionary<string, Function>();
            ReadFunctions();
        }

        public void ReadFunctions()
        {
            using var stream = typeof(FunctionManager).Assembly.GetManifestResourceStream("Common.functions.json");
            using var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            try
            {
                var document = JsonDocument.Parse(content);
                var root = document.RootElement;
                foreach (var function in root.EnumerateArray())
                {
                    var @params = function.GetProperty("parameters");
                    List<Parameter> parameters = new List<Parameter>();
                    foreach (var p in @params.EnumerateArray())
                    {
                        parameters.Add(new Parameter()
                        {
                            Name = p.GetProperty("name").GetString(),
                            Type = TypeSystem.Instance[p.GetProperty("type").GetString()]
                        });
                    }

                    var functionName = function.GetProperty("name").GetString();

                    functions.Add(functionName, new Function()
                    {
                        Name = functionName,
                        ReturnType = TypeSystem.Instance[function.GetProperty("returnType").GetString()],
                        Parameters = parameters
                    });
                }
            }
            catch
            {
                throw new FormatException("The format of function.json is invalid.");
            }
        }

        public Function this[string name]
        {
            get
            {
                if (name == null)
                    return null;

                if (functions.ContainsKey(name))
                    return functions[name];
                throw new KeyNotFoundException("Function not found: " + name);
            }
        }
    }

}
