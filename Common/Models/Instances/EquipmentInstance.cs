using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class EquipmentInstance : ItemInstance
    {
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
