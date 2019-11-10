using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class NpcBase : CharacterBase
    {
        public override Type InstanceType { get { return typeof(Npc); } }
    }
}