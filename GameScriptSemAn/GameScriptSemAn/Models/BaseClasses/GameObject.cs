using GameScript.Models.InstanceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.BaseClasses
{
    public class GameObject
    {
        public virtual Type InstanceType { get { return typeof(GameObjectInstance); } }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }

        public GameObjectInstance Spawn(string instanceId = null)
        {
            GameObjectInstance instance = System.Activator.CreateInstance(InstanceType) as GameObjectInstance;
            instance.Base = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
