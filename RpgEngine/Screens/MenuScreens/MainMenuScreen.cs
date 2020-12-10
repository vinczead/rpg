using Common.Utility;
using Microsoft.Xna.Framework;
using RpgEngine.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem("New Game", "", NewGameMenuSelected),
                new MenuItem("Load Game", "", LoadGameMenuSelected),
                new MenuItem("Credits", "", CreditsMenuSelected),
                new MenuItem("Exit Game", "", OnCancel)
            };
            SetupDefaultMenuItemPositions();

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Draw(Assets.MenuBackground, new Rectangle(0, 0, 320, 200), Color.White);
            base.Draw(gameTime);
        }

        private void CreditsMenuSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen());
        }

        private void NewGameMenuSelected(object sender, EventArgs e)
        {
            var worlds = Directory.GetFiles("Content/World", "*.vgs");
            if (worlds.Length == 1)
            {
                LoadingScreen.Load(ScreenManager, new Screen[] { new GameplayScreen(worlds[0]) });
            }
            else
                ScreenManager.AddScreen(SelectorMenuScreen.Create("Select a world!", worlds, StartWorld));
        }

        private void StartWorld(object sender, MenuItemSelectedEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new Screen[] { new GameplayScreen(e.MenuItem.Text) });
        }

        private void LoadGameMenuSelected(object sender, EventArgs e)
        {
            //todo: implement
        }

        protected override void OnCancel()
        {
            ScreenManager.AddScreen(new ConfirmDialogScreen("Do you really want to exit?", ExitGameConfirmed));
        }

        private void ExitGameConfirmed(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
