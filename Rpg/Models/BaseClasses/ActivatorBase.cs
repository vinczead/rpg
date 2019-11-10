using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rpg.Models
{
    public class ActivatorBase : GameObjectBase
    {
        public override Type InstanceType { get { return typeof(Activator); } }
    }
}