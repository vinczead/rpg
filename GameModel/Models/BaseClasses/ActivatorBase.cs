using System;

namespace GameModel.Models
{
    public class ActivatorBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
    }
}