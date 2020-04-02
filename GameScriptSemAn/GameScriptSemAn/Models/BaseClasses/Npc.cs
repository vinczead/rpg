using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Npc : Character
    {
        public override Type InstanceType { get { return typeof(NpcInstance); } }
    }
}