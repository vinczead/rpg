using GameScript.Models.InstanceClasses;
using System;
using System.Collections.Generic;

namespace GameScript.Models.BaseClasses
{
    public class Character : Creature
    {
        public override Type InstanceType { get { return typeof(CharacterInstance); } }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}