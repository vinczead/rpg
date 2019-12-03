using GameModel.Models.InstanceInterfaces;
using System.Collections.Generic;

namespace Rpg.Models
{
    public class Character : Creature, ICharacter
    {
        public List<IItem> Items { get; set; } = new List<IItem>();

        public void InsertItem(IGameWorldObject gameObject)
        {
            Items.Add(gameObject as Item);
        }

        public void DropItem(IGameWorldObject gameObject)
        {
            Items.Remove(gameObject as Item);
            gameObject.Position = Position;
            Room.InsertGameWorldObject(gameObject);
        }
    }
}