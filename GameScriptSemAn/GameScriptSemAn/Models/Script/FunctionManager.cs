using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public sealed class FunctionManager
    {
        public static FunctionManager Instance { get; } = new FunctionManager();

        private Dictionary<string, Function> functions;

        private FunctionManager()
        {
            functions = new Dictionary<string, Function>();
            ReadFunctions(@"C:\Users\Ady\source\repos\Rpg\GameScriptSemAn\GameScriptSemAn\functions.json");
        }

        public void ReadFunctions(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    var parsedFunctions = (JArray)serializer.Deserialize(jr);

                    foreach (var f in parsedFunctions)
                    {
                        var @params = f.Value<JArray>("parameters");

                        List<Parameter> parameters = new List<Parameter>();
                        foreach (var p in @params)
                        {
                            parameters.Add(new Parameter()
                            {
                                Name = p.Value<string>("name"),
                                Type = TypeSystem.Instance[p.Value<string>("type")]
                            });
                        }

                        var functionName = f.Value<string>("name");

                        functions.Add(functionName, new Function()
                        {
                            Name = functionName,
                            ReturnType = TypeSystem.Instance[f.Value<string>("returnType")],
                            Parameters = parameters
                        });
                    }
                }
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
