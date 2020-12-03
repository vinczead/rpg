using Common.Models;
using Common.Script.Visitors;
using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Screens
{
    public class GameplayScreen : Screen
    {
        string worldScriptFileName;
        public GameplayScreen(string worldScriptFileName)
        {
            this.worldScriptFileName = worldScriptFileName;
        }

        public override void Draw(GameTime gameTime)
        {
            var player = World.Instance.Player;
            var playerX = (int)player.Position.X;
            var playerY = (int)player.Position.Y;
            var playerFrame = player.Breed.Model[player.StateString].FrameAt(player.AnimationTime);
            var playerTranslationMatrix = Matrix.CreateTranslation(Constants.CanvasWidth / 2 - playerX, Constants.CanvasHeight / 2 - playerY + playerFrame.Source.Height / 2, 0);
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, playerTranslationMatrix);
            World.Instance.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            World.Instance.Update(gameTime);
        }

        public override void HandleInput()
        {
            var player = World.Instance.Player;

            var newDirection = player.Direction;
            var newState = State.Idle;

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
                ExecutionVisitor.BuildWorldFromFile(worldScriptFileName, out var _); //todo: push errors to console ?
            }
            ScreenManager.Game.ResetElapsedTime();
            base.LoadContent();
        }
    }
}
