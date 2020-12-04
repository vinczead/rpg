using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgEngine.Screens
{
    class PauseMenuScreen : MenuScreen
    {
        Vector2 menuSize;

        public PauseMenuScreen() : base()
        {
            IsOverlay = true;
            BlockScreenUpdatesBelow = true;

            MenuItems = new List<MenuItem>()
            {
                new MenuItem("Continue", "", OnCancel),
                new MenuItem("Load Game", "", LoadGameMenuSelected),
                new MenuItem("Save Game", "", SaveGameMenuSelected),
                new MenuItem("Credits", "", CreditsMenuSelected),
                new MenuItem("Back to Main Menu", "", ExitGameMenuSelected)
            };
            SetupDefaultMenuItemPositions();
            var menuWidth = (int)MenuItems.Max(item => item.Font.MeasureString(item.Text).X);
            var maxHeight = (int)MenuItems.Max(item => item.Font.MeasureString(item.Text).Y);
            var menuHeight = MenuItems.Count * maxHeight;
            menuSize = new Vector2(menuWidth, menuHeight) * 1.1f;
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredTextureStretched(sb, Assets.TransparentBox, Constants.Canvas / 2, menuSize, Assets.SemiTransparentBlack);

            base.Draw(gameTime);
        }

        private void CreditsMenuSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen());
        }

        private void NewGameMenuSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new Screen[] { new GameplayScreen("Content/World/World.vgs") });
        }

        private void LoadGameMenuSelected(object sender, EventArgs e)
        {
            //todo: implement
        }

        private void SaveGameMenuSelected(object sender, EventArgs e)
        {
            //todo: implement
        }

        private void ExitGameMenuSelected(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.PopScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }
    }
}
