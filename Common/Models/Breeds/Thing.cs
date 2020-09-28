using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Thing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SpriteModel Model { get; set; }
        public virtual Type InstanceType { get { return typeof(ThingInstance); } }
        public virtual ThingInstance Spawn(string instanceId = null)
        {
            ThingInstance instance = Activator.CreateInstance(InstanceType) as ThingInstance;
            
            instance.Breed = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            return instance;
        }
    }
}
