﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CharacterInstance : CreatureInstance
    {
        public List<ItemInstance> Items { get; set; }

        public void DropItem(ItemInstance item)
        {
            Items.Remove(item);
            Map.AddInstance(item);
            item.Position = Position;
        }

        public void PickUpItem(ItemInstance item)
        {
            Map.RemoveInstance(item);
            Items.Add(item);
        }
    }
}
