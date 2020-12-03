using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    class CreditsScreen : Screen
    {
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredText(spriteBatch, Assets.StandardFont, "Credits", new Vector2(Constants.CanvasWidth / 2, 20), Assets.StandardTextColor);
            GuiHelper.DrawCenteredText(spriteBatch, Assets.StandardFont, "Created by Adam Vincze in 2020", new Vector2(Constants.CanvasWidth / 2, 70), Assets.StandardTextColor);
            GuiHelper.DrawCenteredText(spriteBatch, Assets.StandardFont, "Powered by MonoGame", new Vector2(Constants.CanvasWidth / 2, 120), Assets.StandardTextColor);

            spriteBatch.DrawString(Assets.StandardFont, "[Esc] Back", new Vector2(0, Constants.CanvasHeight - 15), Assets.StandardTextColor);
        }

        public override void HandleInput()
        {
            if (InputHandler.WasActionJustReleased(InputHandler.Action.Back))
                ScreenManager.RemoveScreen(this);
        }
    }
}
