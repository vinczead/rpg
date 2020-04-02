using System.Collections.Generic;

namespace GameScript.Models.InstanceClasses
{
    public class CharacterInstance : CreatureInstance
    {
        public List<ItemInstance> Items { get; set; } = new List<ItemInstance>();

        public void InsertItem(ItemInstance item)
        {
            Items.Add(item);
            Executer.ExecuteRunBlock(this, "WhenItemPickedUp");
        }

        public void DropItem(ItemInstance item)
        {
            Items.Remove(item);
            item.Drop(this);
            Executer.ExecuteRunBlock(this, "WhenItemDropped");
        }
    }
}