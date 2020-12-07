using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Creature : Thing
    {
        public override Type InstanceType { get { return typeof(CreatureInstance); } }
        
        //Base Stats
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }

        //Base Attributes
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Speed { get; set; }
        public int Intelligence { get; set; }

        //Natural Protection
        public int Protection { get; set; }

        public override ThingInstance Spawn(string instanceId = null)
        {
            var instance = base.Spawn(instanceId) as CreatureInstance;

            instance.CurrentHealth = MaxHealth/2;
            instance.CurrentMana = MaxMana;

            return instance;
        }
    }
}
