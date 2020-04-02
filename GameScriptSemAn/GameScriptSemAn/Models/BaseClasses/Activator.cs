using GameScript.Models.InstanceClasses;
using System;

namespace GameScript.Models.BaseClasses
{
    public class Activator : Thing
    {
        public override Type InstanceType { get { return typeof(ActivatorInstance); } }
    }
}