using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Model
{
    public class TypeSystem
    {
        Dictionary<string, Type> types;

        public TypeSystem()
        {
            types = new Dictionary<string, Type>();
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
                    }
                    foreach (var type in parsedTypes)
                    {
                        var currentTypeName = type.Value<string>("name");

                        var events = type.Value<JArray>("events") ?? new JArray();
                        var parents = type.Value<JArray>("parents") ?? new JArray();
                        var functions = type.Value<JArray>("functions") ?? new JArray();
                        var properties = type.Value<JArray>("properties") ?? new JArray();

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

                        types[currentTypeName].Functions = functions.Select(f =>
                        {
                            var @params = f.Value<JArray>("parameters");

                            List<Parameter> parameters = new List<Parameter>();
                            foreach (var p in @params)
                            {
                                parameters.Add(new Parameter()
                                {
                                    Name = p.Value<string>("name"),
                                    Type = types[p.Value<string>("type")]
                                });
                            }

                            return new Function()
                            {
                                Name = f.Value<string>("name"),
                                ReturnType = types[f.Value<string>("returnType")],
                                Parameters = parameters
                            };
                        }).ToHashSet();
                    }
                }
            }
        }

        public Type this[string name]
        {
            get
            {
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
