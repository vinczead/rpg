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
                _ => throw new ArgumentException("No Base can be created with the specified name.", "name"),
            };
        }
    }
}
