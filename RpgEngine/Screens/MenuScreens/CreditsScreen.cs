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

            spriteBatch.Begin();
            GuiHelper.DrawCenteredText(spriteBatch, CommonAssets.StandardFont, "Credits", new Vector2(Constants.CanvasWidth / 2, 20), CommonAssets.StandardTextColor);
            GuiHelper.DrawCenteredText(spriteBatch, CommonAssets.StandardFont, "Created by Adam Vincze in 2020", new Vector2(Constants.CanvasWidth / 2, 70), CommonAssets.StandardTextColor);
            GuiHelper.DrawCenteredText(spriteBatch, CommonAssets.StandardFont, "Powered by MonoGame", new Vector2(Constants.CanvasWidth / 2, 120), CommonAssets.StandardTextColor);

            spriteBatch.DrawString(CommonAssets.StandardFont, "[Esc] Back", new Vector2(0, Constants.CanvasHeight - 15), CommonAssets.StandardTextColor);
            spriteBatch.End();
        }

        public override void HandleInput()
        {
            if (InputHandler.WasActionJustReleased(InputHandler.Action.Back))
                ScreenManager.RemoveScreen(this);
        }
    }
}
