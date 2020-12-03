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
    class ConfirmDialogScreen : MenuScreen
    {
        private string message;

        public ConfirmDialogScreen(string message, EventHandler yesHandler = null, EventHandler noHandler = null)
        {
            IsOverlay = true;
            this.message = message;

            var noMenu = new MenuItem("No")
            {
                Description = "",
                Font = Assets.StandardFont
            };
            var noMenuPosX = (int)(Constants.CanvasWidth / 2 - Assets.StandardFont.MeasureString(noMenu.Text).X / 2);
            noMenu.Position = new Vector2(noMenuPosX, 100);
            if (noHandler != null)
                noMenu.Selected += noHandler;
            noMenu.Selected += OnCancel;
            MenuItems.Add(noMenu);

            var yesMenu = new MenuItem("Yes")
            {
                Description = "",
                Font = Assets.StandardFont,
            };
            var yesMenuPosX = (int)(Constants.CanvasWidth / 2 - Assets.StandardFont.MeasureString(yesMenu.Text).X / 2);
            yesMenu.Position = new Vector2(yesMenuPosX, 115);
            if (yesHandler != null)
                yesMenu.Selected += yesHandler;
            MenuItems.Add(yesMenu);
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredTexture(sb, Assets.PopupBackground, new Vector2(Constants.CanvasWidth / 2, Constants.CanvasHeight / 2), Color.White);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, message, new Vector2(Constants.CanvasWidth / 2, 85), Assets.StandardTextColor);

            base.Draw(gameTime);
        }
    }
}
