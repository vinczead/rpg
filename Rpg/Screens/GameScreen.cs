using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameScript.Models;
using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using GameScript.Visitors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Screens
{
    public class GameScreen : Screen
    {
        public GameModel gameModel { get; set; }

        public GameScreen(RpgGame game) : base(game)
        {
            var scriptFile = new ScriptFile()
            {
                Document = new ICSharpCode.AvalonEdit.Document.TextDocument()
                {
                    Text = @"
Base $XPCollectorWeapon From Equipment
	Set Name ""Sword of Experience Collection""
	Set Description ""This sword gets stronger with every slain enemy.""
	Set Value 1000
	Set Weight 5
	SetTexture($XPCollectorWeapon, ""Sword"")

	Variables
		CollectedXP Is Number
	End


	Run When TargetDead
		Set CollectedXP To CollectedXP + GetXPValue(Target)
	End
End

Base $RingOfStrength From Equipment
	Set Name ""Ring of Strength""
	Set Description ""Increases Strength by 5""
	Set Value 100
	Set Weight 1
	SetTexture($RingOfStrength, ""Ring"")


	Run When Equipped
		Set Actor.BonusStrength To Actor.BonusStrength + 5
	End


	Run When Unequipped
		Set Actor.BonusStrength To Actor.BonusStrength - 5
	End
End

Base $HealthPotion From Consumable
	Set Name ""Potion of Health""
	Set Description ""This potion restores 50 points of health.""
	Set Weight 2 + Random()
	Set Value 100
	SetTexture($HealthPotion, ""Potion"")


	Variables
		randomvar is Number
	End


	Run When Consumed
		//SetVar $sadasd sad 5
		//SetVar Self randomvar 5
		//SetVar $Player collecteditems 5
					Set randomvar 5
		Set Actor.CurrentHealth To Actor.CurrentHealth + 50
	End
End

Base $Protagonist From Player
	SetTexture($Protagonist, ""Player"")
End

Region $Dungeon1
	//Set Width 14
	//Set Height 14
	//SetTiles(Self, 1, 1, 14, 14, ""Grass"")

	Instance $PlayerInstance From $Protagonist
		Set X 1
		Set Y 4
	End

	Instance $HP1 From $HealthPotion
		Set X 5
		Set Y 10
	End


	Instance From $HealthPotion
		Set X 6
		Set Y 11
	End


	Instance From $RingOfStrength
		Set X 2
		Set Y 2
	End
End
"
				}
            };

            gameModel = ExecutionVisitor.Build(new List<ScriptFile>() { scriptFile}, out _);
			gameModel.Player = gameModel.Instances["$PlayerInstance"] as PlayerInstance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameModel.Draw(gameTime, spriteBatch);
            if (releasedKeys.Length > 0)
                Console.WriteLine(releasedKeys.Length);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gameModel.Update(gameTime);
            if (WasKeyPressed(Keys.Space))
            {
                ThingInstance selectedThing = null;

                if (selectedThing is ItemInstance)
                {
                    (selectedThing as ItemInstance).PickUp(gameModel.Player);
                }
            }
            if (WasKeyPressed(Keys.F12))
            {
                game.AddScreen(new ConsoleScreen(game, gameModel));
            }

            if (WasKeyPressed(Keys.I))
            {
                game.AddScreen(new InventoryScreen(game, gameModel));
            }
        }
    }
}