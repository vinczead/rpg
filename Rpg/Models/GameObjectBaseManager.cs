using GameModel.Models.BaseInterfaces;
using Rpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public static class GameObjectBaseManager
    {
        public static Dictionary<string, IGameObjectBase> GameObjectBases { get; set; } = new Dictionary<string, IGameObjectBase>();

        static GameObjectBaseManager()
        {
            PlayerBase player = new PlayerBase()
            {
                Id = "Player",
                Name = "You",
                Texture = TextureManager.player,
                Health = 100,
                Strength = 10
            };

            EquipmentBase ringOfExample = new EquipmentBase()
            {
                Id = "RingOfExample",
                Name = "Ring of Example",
                Script = @"Script RingOFExampleScript

Variables
	AlreadyPickedUp Is Boolean WithValue False

Run WhenPickedUp
	If AlreadyPickedUp = False Then
		Set AlreadyPickedUp To True
		$Player=>SetHealth($Player=>GetHealth() + 1)
	EndIf
End",
                Texture=TextureManager.ring,
                Type= EquipmentType.Ring
                };

            GameObjectBases.Add(player.Id, player);
            GameObjectBases.Add(ringOfExample.Id, ringOfExample);
        }
    }
}
