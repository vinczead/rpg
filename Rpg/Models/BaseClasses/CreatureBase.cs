using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class CreatureBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
        public int Health { get; set; }
        public int Strength { get; set; }
    }
}