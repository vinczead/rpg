using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ItemInstance : ThingInstance
    {
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
