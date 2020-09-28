using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Item : Thing
    {
        public override Type InstanceType { get { return typeof(ItemInstance); } }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
