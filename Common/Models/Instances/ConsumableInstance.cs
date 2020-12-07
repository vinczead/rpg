using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ConsumableInstance : ItemInstance
    {
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
