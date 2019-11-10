using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class CharacterBase : CreatureBase
    {
        public override Type InstanceType { get { return typeof(Character); } }
        public List<ItemBase> Items { get; set; } = new List<ItemBase>();
    }
}