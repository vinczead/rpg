﻿using Common.Models;
using Common.Script.Visitors;
using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgEngine.Screens
{
    class GameplayScreen : Screen
    {
        string worldScriptFileName;

        private TimeSpan messageTimer = TimeSpan.Zero;
        public GameplayScreen(string worldScriptFileName)
        {
            this.worldScriptFileName = worldScriptFileName;
        }

        public override void Draw(GameTime gameTime)
        {
            var player = World.Instance.Player;
            var playerX = (int)player.Position.X;
            var playerY = (int)player.Position.Y;
            var playerFrame = player?.Breed?.Model[player.StateString]?.FrameAt(player.AnimationTime) ?? Animation.Empty.Frames.First();
            var playerTranslationMatrix = Matrix.CreateTranslation(Constants.CanvasWidth / 2 - playerX, Constants.CanvasHeight / 2 - playerY + playerFrame.Source.Height / 2, 0);

            var sb = ScreenManager.SpriteBatch;
            sb.End();

            //World should be drawn translated
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, playerTranslationMatrix);
            World.Instance.Draw(ScreenManager.SpriteBatch);

            var playerClosestInstance = World.Instance.Player.ClosestInstance;
            if (playerClosestInstance != null)
            {
                var underInstance = new Vector2(playerClosestInstance.Position.X, playerClosestInstance.Position.Y - 5 - 32);
                GuiHelper.DrawCenteredText(sb, Assets.StandardFont, playerClosestInstance.Breed.Name, underInstance, Assets.HighlightedTextColor);
            }
            sb.End();

            sb.Begin();
            DrawHud(sb);
        }

        private void DrawHud(SpriteBatch sb)
        {
            for (int i = 0; i < EngineVariables.Messages.Count; i++)
            {
                var message = EngineVariables.Messages.ElementAt(i);
                sb.DrawString(Assets.StandardFont, message, new Vector2(5, 5 + i * 15), Assets.StandardTextColor);
            }

            var player = World.Instance.Player;
            var maxHealth = (player.Breed as Character).MaxHealth;
            var currentHealth = (int)(100.0 * player.CurrentHealth / maxHealth);

            var maxHealthBarRectangle = new Rectangle(5, Constants.CanvasHeight - 15, 100, 10);
            sb.Draw(Assets.TransparentBox, maxHealthBarRectangle, Color.Red);

            var healthBarRectangle = new Rectangle(5, Constants.CanvasHeight - 15, currentHealth, 10);
            sb.Draw(Assets.SolidBox, healthBarRectangle, Color.Red);

            var maxMana = (player.Breed as Character).MaxMana;
            var currentMana = (int)(100.0 * player.CurrentMana / maxMana);

            var maxManaBarRectangle = new Rectangle(Constants.CanvasWidth - 105, Constants.CanvasHeight - 15, 100, 10);
            sb.Draw(Assets.TransparentBox, maxManaBarRectangle, Color.Blue);

            var manaBarRectangle = new Rectangle(Constants.CanvasWidth - 105, Constants.CanvasHeight - 15, currentMana, 10);
            sb.Draw(Assets.SolidBox, manaBarRectangle, Color.Blue);

            var playerClosestInstance = World.Instance.Player.ClosestInstance;
            if (playerClosestInstance is CreatureInstance)
            {
                var closestCreature = playerClosestInstance as CreatureInstance;
                maxHealth = (closestCreature.Breed as Creature).MaxHealth;
                currentHealth = (int)(100.0 * closestCreature.CurrentHealth / maxHealth);

                maxHealthBarRectangle = new Rectangle(Constants.CanvasWidth / 2 - 50, Constants.CanvasHeight - 15, 100, 10);
                sb.Draw(Assets.TransparentBox, maxHealthBarRectangle, Color.Red);

                healthBarRectangle = new Rectangle(Constants.CanvasWidth / 2 - 50, Constants.CanvasHeight - 15, currentHealth, 10);
                sb.Draw(Assets.SolidBox, healthBarRectangle, Color.Red);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (messageTimer.TotalMilliseconds > 3000)
            {
                EngineVariables.Messages.Dequeue();
                messageTimer = TimeSpan.Zero;
            }
            if (EngineVariables.Messages.Count > 0)
                messageTimer += gameTime.ElapsedGameTime;
            World.Instance.Update(gameTime);
        }

        public override void HandleInput()
        {
            var player = World.Instance.Player;

            var newDirection = player.Direction;
            var newState = State.Idle;

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Back))
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }

            if (InputHandler.WasActionJustReleased(InputHandler.Action.Inventory))
            {
                ScreenManager.AddScreen(new InventoryScreen());
            }
            else
            {
                if (InputHandler.IsActionPressed(InputHandler.Action.Left))
                {
                    newDirection = Direction.Left;
                    newState = State.Move;
                }
                if (InputHandler.IsActionPressed(InputHandler.Action.Right))
                {
                    newDirection = Direction.Right;
                    newState = State.Move;
                }

                if (InputHandler.IsActionPressed(InputHandler.Action.Up))
                {
                    newDirection = Direction.Up;
                    newState = State.Move;
                }
                if (InputHandler.IsActionPressed(InputHandler.Action.Down))
                {
                    newDirection = Direction.Down;
                    newState = State.Move;
                }

                if (InputHandler.WasActionJustReleased(InputHandler.Action.Action))
                {
                    if (player.ClosestInstance != null)
                    {
                        var pickUpVisitor = new PickUpVisitor(World.Instance.Player);
                        player.ClosestInstance.Accept(pickUpVisitor);
                        if (pickUpVisitor.Success)
                            return; //maybe not?

                        var activateVisitor = new ActivateVisitor(World.Instance.Player);
                        player.ClosestInstance.Accept(activateVisitor);
                        if (activateVisitor.Success)
                            return;

                        if (player.ClosestInstance is CharacterInstance)
                        {
                            ScreenManager.AddScreen(new ConversationScreen(player.ClosestInstance as CharacterInstance));
                        }
                    }
                }
                if (InputHandler.WasActionJustReleased(InputHandler.Action.Attack))
                {
                    if(player.ClosestInstance != null)
                    {
                        player.ClosestInstance.Accept(new MeleeAttackVisitor(World.Instance.Player));
                    }
                }
            }

            if (player.State != newState || player.Direction != newDirection)
            {
                player.Direction = newDirection;
                player.State = newState;
                player.AnimationTime = TimeSpan.Zero;
            }

            if (InputHandler.WasKeyJustReleased(Keys.F9))
            {
                EngineVariables.ShowEntityBoundingBox = !EngineVariables.ShowEntityBoundingBox;
            }

            if (InputHandler.WasKeyJustReleased(Keys.F10))
            {
                EngineVariables.ShowEntityCollisionBox = !EngineVariables.ShowEntityCollisionBox;
            }
        }

        public override void LoadContent()
        {
            if (worldScriptFileName != null)
            {
                World.Instance.Game = ScreenManager.Game;
                ExecutionVisitor.BuildWorldFromFile(worldScriptFileName, out var messages);
                EngineVariables.ConsoleContents.AddRange(messages.Select(message => message.ToString()));
            }
            ScreenManager.Game.ResetElapsedTime();
            base.LoadContent();
        }
    }
}
