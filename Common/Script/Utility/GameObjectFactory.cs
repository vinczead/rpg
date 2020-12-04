using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.Utility
{
    public static class GameObjectFactory
    {
        public static Thing Create(string name)
        {
            return name switch
            {
                "Thing" => new Thing(),
                "Creature" => new Creature(),
                "Character" => new Character(),
                "Item" => new Item(),
                "Consumable" => new Consumable(),
                "Activator" => new Models.Activator(),
                _ => throw new ArgumentException("No Breed can be created with the specified name.", "name"),
            };
        }
    }
}
