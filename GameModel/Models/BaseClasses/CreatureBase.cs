using System;

namespace GameModel.Models
{
    public class CreatureBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
        public int Health { get; set; }
        public int Strength { get; set; }
    }
}