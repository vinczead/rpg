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
                Strength = 10,
                Script = @"Script PlayerScript
Variables 
    A Is Number WithValue 1000
Run WhenPickedUpItem
    =>Message(""Item has been added to inventory."")
End
"
            };

            EquipmentBase ringOfExample = new EquipmentBase()
            {
                Id = "RingOfExample",
                Name = "Ring of Example",
                Script = @"Script RingOFExampleScript

Variables
	AlreadyPickedUp Is Boolean WithValue False
    PickUpCount Is Number WithValue 0

Run WhenPickedUp
    $PlayerInstance=>SetHealth($PlayerInstance=>GetHealth()+5)
    =>Message(""Health increased by 5!"")
    Set PickUpCount To PickUpCount + 1
	If AlreadyPickedUp = False Then
		Set AlreadyPickedUp To True
		=>Message(""Ring of Example picked up the first time."")
    Else
        =>Message(""Ring of Example picked up this many times: "" + PickUpCount)
	EndIf
End

Run WhenDropped
    $PlayerInstance=>SetHealth($PlayerInstance=>GetHealth()-5)
    =>Message(""Health decreased by 5!"")
End
",
                Texture=TextureManager.ring,
                Type= EquipmentType.Ring
                };

            GameObjectBases.Add(player.Id, player);
            GameObjectBases.Add(ringOfExample.Id, ringOfExample);
        }
    }
}
