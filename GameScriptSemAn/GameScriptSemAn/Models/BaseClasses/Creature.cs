using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Creature : Thing
    {
        public override Type InstanceType { get { return typeof(CreatureInstance); } }
        public int MaxHealth { get; set; }
        public int Strength { get; set; }
    }
}