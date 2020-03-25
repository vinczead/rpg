using System;

namespace Rpg.Models
{
    public class CreatureBase : GameWorldObjectBase
    {
        public override Type InstanceType { get { return typeof(Creature); } }
        public int Health { get; set; }
        public int Strength { get; set; }
    }
}