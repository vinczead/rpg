using System;

namespace Rpg.Models
{
    public class CreatureBase : GameWorldObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
        public int Health { get; set; }
        public int Strength { get; set; }
    }
}