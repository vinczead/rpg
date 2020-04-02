using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Player : Character
    {
        public override Type InstanceType { get { return typeof(PlayerInstance); } }
    }
}