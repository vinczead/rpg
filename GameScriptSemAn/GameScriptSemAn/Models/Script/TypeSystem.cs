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
    public sealed class TypeSystem
    {
        public static TypeSystem Instance { get; } = new TypeSystem();

        private Dictionary<string, Type> types = new Dictionary<string, Type>();

        private TypeSystem()
        {
            AddSimpleTypes();
            ReadTypes(@"C:\Users\Adam\sources\rpg\GameScriptSemAn\GameScriptSemAn\builtintypes.json");
        }

        public void AddSimpleTypes()
        {
            types.Add("ErrorType", new Type("ErrorType"));
            types.Add("AnyType", new Type("AnyType"));
            types.Add("String", new Type("String"));
            types.Add("Boolean", new Type("Boolean"));
            types.Add("Number", new Type("Number"));
            types.Add("NullType", new Type("NullTypfe"));
            
            types.Add("Array", new Type("Array"));
            types.Add("StringArray", new Type("StringArray"));
            types.Add("BooleanArray", new Type("BooleanArray"));
            types.Add("NumberArray", new Type("NumberArray"));

            this["ErrorType"].Parents.Add(this["AnyType"]);
            
            this["AnyType"].Parents.Add(this["String"]);
            this["AnyType"].Parents.Add(this["Boolean"]);
            this["AnyType"].Parents.Add(this["Number"]);
            this["AnyType"].Parents.Add(this["NullType"]);
            this["AnyType"].Parents.Add(this["Array"]);

            this["Array"].Parents.Add(this["StringArray"]);
            this["Array"].Parents.Add(this["BooleanArray"]);
            this["Array"].Parents.Add(this["NumberArray"]);
        }

        public void ReadTypes(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    var parsedTypes = (JArray)serializer.Deserialize(jr);
                    foreach (var type in parsedTypes)
                    {
                        var name = type.Value<string>("name");
                        types.Add(name, new Type(name));
                        types.Add($"{name}Array", new Type($"{name}Array"));
                        this["Array"].Parents.Add(this[$"{name}Array"]);
                    }
                    foreach (var type in parsedTypes)
                    {
                        var currentTypeName = type.Value<string>("name");

                        var events = type.Value<JArray>("events") ?? new JArray();
                        var parents = type.Value<JArray>("parents") ?? new JArray();
                        var parentOfNull = type.Value<bool>("parentOfNull");
                        var properties = type.Value<JArray>("properties") ?? new JArray();

                        if (parentOfNull)
                            types["NullType"].Parents.Add(types[currentTypeName]);

                        types[currentTypeName].Parents = parents.Select(p => types[p.Value<string>()]).ToHashSet();
                        types[currentTypeName].Events = events.Select(e =>
                        {
                            var @params = e.Value<JArray>("parameters");

                            List<Parameter> parameters = new List<Parameter>();
                            foreach (var p in @params)
                            {
                                parameters.Add(new Parameter()
                                {
                                    Name = p.Value<string>("name"),
                                    Type = types[p.Value<string>("type")]
                                });
                            }

                            return new Event()
                            {
                                Name = e.Value<string>("name"),
                                Parameters = parameters
                            };
                            }).ToHashSet();

                        types[currentTypeName].Properties = properties.Select(p => new Property()
                        {
                            Name = p.Value<string>("name"),
                            Type = types[p.Value<string>("type")]
                        }).ToHashSet();
                    }
                }
            }
        }

        public Type this[string name]
        {
            get
            {
                if (name == null)
                    return null;

                if (types.ContainsKey(name))
                    return types[name];
                throw new KeyNotFoundException("Type not found: " + name);
            }
        }

        public Type CreateType(string name, Type descendsFrom)
        {
            if (types.ContainsKey(name))
                throw new ArgumentException("Type already defined: " + name);
            var type = new Type(name);
            types[name] = type;
            descendsFrom.Parents.Add(type);
            return type;
        }
    }
}
