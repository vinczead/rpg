using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Script.Utility
{
    public sealed class TypeSystem
    {
        public static TypeSystem Instance { get; } = new TypeSystem();

        private Dictionary<string, Type> types = new Dictionary<string, Type>();

        private TypeSystem()
        {
            AddSimpleTypes();
            ReadTypes();
        }

        public void AddSimpleTypes()
        {
            types.Add("ErrorType", new Type("ErrorType"));
            types.Add("AnyType", new Type("AnyType"));
            types.Add("String", new Type("String"));
            types.Add("Boolean", new Type("Boolean"));
            types.Add("Number", new Type("Number"));
            types.Add("NullType", new Type("NullType"));

            types.Add("Array", new Type("Array"));
            types.Add("NullTypeArray", new Type("NullTypeArray"));
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
            this["Array"].Parents.Add(this["NullTypeArray"]);
        }

        public void ReadTypes()
        {
            using var stream = typeof(FunctionManager).Assembly.GetManifestResourceStream("Common.types.json");
            using var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            try
            {
                var document = JsonDocument.Parse(content);
                var root = document.RootElement;

                foreach (var type in root.EnumerateArray())
                {
                    var name = type.GetProperty("name").GetString();
                    types.Add(name, new Type(name));
                    types.Add($"{name}Array", new Type($"{name}Array"));
                    this["NullTypeArray"].Parents.Add(this[$"{name}Array"]);
                }
                foreach (var type in root.EnumerateArray())
                {
                    var currentTypeName = type.GetProperty("name").GetString();

                    if (type.TryGetProperty("parentOfNull", out var parentOfNull) && parentOfNull.GetBoolean())
                        types["NullType"].Parents.Add(types[currentTypeName]);

                    if (type.TryGetProperty("parents", out var parents))
                    {
                        var parentTypes = parents.EnumerateArray().Select(p => types[p.GetString()]);
                        types[currentTypeName].Parents = parentTypes.ToHashSet();
                    }

                    if (type.TryGetProperty("events", out var events))
                    {
                        var eventValues = events.EnumerateArray().Select(e =>
                        {
                            var @params = e.GetProperty("parameters");

                            List<Parameter> parameters = new List<Parameter>();
                            foreach (var p in @params.EnumerateArray())
                            {
                                parameters.Add(new Parameter()
                                {
                                    Name = p.GetProperty("name").GetString(),
                                    Type = types[p.GetProperty("type").GetString()]
                                });
                            }

                            return new Event()
                            {
                                Name = e.GetProperty("name").GetString(),
                                Parameters = parameters
                            };
                        });
                        types[currentTypeName].Events = eventValues.ToHashSet();
                    }

                    if(type.TryGetProperty("properties", out var properties))
                    {
                        var propertyValues = properties.EnumerateArray().Select(p => new Property()
                        {
                            Name = p.GetProperty("name").GetString(),
                            Type = types[p.GetProperty("type").GetString()]
                        });
                        types[currentTypeName].Properties = propertyValues.ToHashSet();
                    }
                }
            }
            catch
            {
                throw new FormatException("The format of types.json is invalid.");
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
