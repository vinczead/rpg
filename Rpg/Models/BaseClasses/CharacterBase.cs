using System;
using System.Collections.Generic;

namespace Rpg.Models
{
    public class CharacterBase : CreatureBase
    {
        public override Type InstanceType { get { return typeof(Character); } }
        public List<ItemBase> Items { get; set; } = new List<ItemBase>();
    }
}