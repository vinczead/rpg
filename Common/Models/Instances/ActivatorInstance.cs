using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ActivatorInstance : ThingInstance
    {
        private bool activated;
        public bool Activated
        {
            get => activated; set
            {
                activated = value;
                AnimationTime = TimeSpan.Zero;
            }
        }
        public override string StateString => Activated ? "Active" : "Inactive";

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
