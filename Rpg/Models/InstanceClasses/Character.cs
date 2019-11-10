using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public class Character : Creature
    {
        public List<Item> Items { get; set; } = new List<Item>();

        public void InsertItem(GameObject gameObject)
        {
            Items.Add(gameObject as Item);
        }

        public void DropItem(GameObject gameObject)
        {
            Items.Remove(gameObject as Item);
            gameObject.Position = Position;
            Room.InsertGameObject(gameObject);
        }
    }
}