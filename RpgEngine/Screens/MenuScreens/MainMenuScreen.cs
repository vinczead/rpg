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
