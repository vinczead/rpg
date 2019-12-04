using GameModel.Models.InstanceInterfaces;
using GameScript;
using System.Collections.Generic;

namespace Rpg.Models
{
    public class Character : Creature, ICharacter
    {
        public List<IItem> Items { get; set; } = new List<IItem>();

        public void InsertItem(IItem item)
        {
            Items.Add(item);
            Executer.ExecuteRunBlock(this, "WhenItemPickedUp");
        }

        public void DropItem(IItem item)
        {
            Items.Remove(item);
            item.Drop(this);
            Executer.ExecuteRunBlock(this, "WhenItemDropped");
        }
    }
}