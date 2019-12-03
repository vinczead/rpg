using System;

namespace Rpg.Models
{
    public class ActivatorBase : GameWorldObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
    }
}