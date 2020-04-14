using GameScript.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models
{
    public class GameObjectFactory
    {
        public static GameObject CreateGameObject(string name)
        {
            switch (name)
            {
                case "Thing":
                    return new Thing();
                case "Activator":
                    return new BaseClasses.Activator();
                case "Creature":
                    return new Creature();
                case "Npc":
                    return new Npc();
                case "Player":
                    return new Player();
                case "Equipment":
                    return new Equipment();
                case "Consumable":
                    return new Consumable();
                default:
                    throw new ArgumentException("No Base can be created with the specified name.", "name");
            }
        }
    }
}
