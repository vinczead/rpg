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

        public ConfirmDialogScreen(string message, EventHandler<MenuItemSelectedEventArgs> yesHandler = null, EventHandler<MenuItemSelectedEventArgs> noHandler = null)
        {
            IsOverlay = true;
            this.message = message;

            var noMenu = new MenuItem("No")
            {
                Description = "",
                Font = Assets.StandardFont
            };
            var measuredString = Assets.StandardFont.MeasureString(noMenu.Text);

            var noMenuPosX = (int)(Constants.CanvasWidth / 2 - measuredString.X / 2);
            noMenu.Position = new Vector2(noMenuPosX, Constants.CanvasHeight / 2);
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
            yesMenu.Position = new Vector2(yesMenuPosX, Constants.CanvasHeight / 2 + measuredString.Y);
            if (yesHandler != null)
                yesMenu.Selected += yesHandler;
            MenuItems.Add(yesMenu);
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            var measuredString = Assets.StandardFont.MeasureString(message);

            GuiHelper.DrawCenteredTextureStretched(sb, Assets.SolidBox, new Vector2(Constants.CanvasWidth / 2, Constants.CanvasHeight / 2), new Vector2(250, 80), Assets.TranslucentBlack1);
            GuiHelper.DrawCenteredText(sb, Assets.StandardFont, message, new Vector2(Constants.CanvasWidth / 2, Constants.CanvasHeight / 2 - measuredString.Y), Assets.StandardTextColor);

            base.Draw(gameTime);
        }
    }
}
