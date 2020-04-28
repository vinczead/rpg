using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameScript.ViGaSParser;

namespace GameScript.Models.BaseClasses
{
    public class GameObject
    {
        public virtual System.Type InstanceType { get { return typeof(GameObjectInstance); } }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, RunBlockContext> RunBlocks { get; set; } = new Dictionary<string, RunBlockContext>();
        public Dictionary<string, Symbol> Variables { get; set; } = new Dictionary<string, Symbol>();

        public GameObjectInstance Spawn(string instanceId = null)
        {
            GameObjectInstance instance = System.Activator.CreateInstance(InstanceType) as GameObjectInstance;
            foreach (var var in Variables.Values)
            {
                instance.Variables.Add(var.Name, new Symbol(var));
            }
            instance.Base = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
