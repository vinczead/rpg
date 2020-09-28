using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Character : Creature
    {
        public override Type InstanceType { get { return typeof(CharacterInstance); } }
        public List<Item> Items { get; set; }
    }
}
