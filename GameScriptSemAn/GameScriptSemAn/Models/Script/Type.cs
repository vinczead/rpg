using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public class Type
    {
        public string Name { get; set; }
        public HashSet<Type> Parents { get; set; } = new HashSet<Type>();

        private HashSet<Event> events;

        public HashSet<Event> Events
        {
            get {
                if (Parents.Count > 0)
                    return Parents
                        .Select(p => p.Events)
                        .Aggregate((result, item) => result.Concat(item).ToHashSet())
                        .Concat(events)
                        .ToHashSet();
                else
                    return events;
            }
            set { events = value; }
        }

        private HashSet<Property> properties;

        public HashSet<Property> Properties
        {
            get
            {
                if (Parents.Count > 0)
                    return properties.Concat(Parents
                        .Select(p => p.Properties)
                        .Aggregate((result, item) => result.Concat(item).ToHashSet()))
                        .ToHashSet();
                else
                    return properties;
            }
            set { properties = value; }
        }

        public Type(string name)
        {
            Name = name;

            Events = new HashSet<Event>();
            Properties = new HashSet<Property>();
        }

        public bool InheritsFrom(Type t)
        {
            return this == t || Parents.Any(p => p.InheritsFrom(t));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
