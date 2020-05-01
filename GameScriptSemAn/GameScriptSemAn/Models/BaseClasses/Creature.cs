﻿using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Creature : Thing
    {
        public override Type InstanceType { get { return typeof(CreatureInstance); } }
        public int Health { get; set; }
        public int Strength { get; set; }
    }
}