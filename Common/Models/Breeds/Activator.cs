using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Activator : Thing
    {
        public override Type InstanceType { get { return typeof(ActivatorInstance); } }
    }
}
