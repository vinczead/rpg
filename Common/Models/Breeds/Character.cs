using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Character : Creature
    {
        public override Type InstanceType { get { return typeof(CharacterInstance); } }
        public List<Item> Items { get; set; } = new List<Item>();
        public Dictionary<string, string> Topics { get; set; } = new Dictionary<string, string>();

        public override ThingInstance Spawn(string instanceId = null)
        {
            var instance = base.Spawn(instanceId) as CharacterInstance;

            foreach (var item in Items)
            {
                var itemInstance = item.Spawn() as ItemInstance;
                instance.Items.Add(itemInstance);
            }
            foreach (var topic in Topics)
            {
                instance.Topics.Add(topic.Key, topic.Value);
            }

            return instance;
        }
    }
}
