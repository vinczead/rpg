using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CharacterInstance : CreatureInstance
    {
        public List<ItemInstance> Items { get; set; } = new List<ItemInstance>();

        public void DropItem(ItemInstance item)
        {
            Items.Remove(item);
            Region.AddInstance(item);
            item.Position = Position;   //todo: avoid collision? (items should not be collidible tho')
            item.Dropped(this);
        }

        public void PickUpItem(ItemInstance item)
        {
            Region.RemoveInstance(item);
            Items.Add(item);
            item.PickedUp(this);
        }
    }
}
