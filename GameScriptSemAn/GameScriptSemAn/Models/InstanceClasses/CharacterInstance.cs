using GameScript.Visitors;
using System.Collections.Generic;

namespace GameScript.Models.InstanceClasses
{
    public class CharacterInstance : CreatureInstance
    {
        public List<ItemInstance> Items { get; set; } = new List<ItemInstance>();

        public void InsertItem(ItemInstance item)
        {
            Items.Add(item);
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "ItemPickedUp");
        }

        public void DropItem(ItemInstance item)
        {
            Items.Remove(item);
            item.Drop(this);
            ExecutionVisitor.ExecuteRunBlock(Region.GameModel, this, "ItemDropped");
        }
    }
}