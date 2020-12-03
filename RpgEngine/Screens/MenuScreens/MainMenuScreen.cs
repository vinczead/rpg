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
            var newGameMenu = new MenuItem("New Game")
            {
                Description = "Start a new game",
                Font = CommonAssets.StandardFont,
            };
            var newGameMenuPosX = (int)(Constants.CanvasWidth / 2 - CommonAssets.StandardFont.MeasureString(newGameMenu.Text).X / 2);
            newGameMenu.Position = new Vector2(newGameMenuPosX, 85);
            newGameMenu.Selected += NewGameMenuSelected;
            MenuItems.Add(newGameMenu);

            var loadGameMenu = new MenuItem("Load Game")
            {
                Description = "Start a new game",
                Font = CommonAssets.StandardFont
            };
            var loadGameMenuPosX = (int)(Constants.CanvasWidth / 2 - CommonAssets.StandardFont.MeasureString(loadGameMenu.Text).X / 2);
            loadGameMenu.Position = new Vector2(loadGameMenuPosX, 100);
            loadGameMenu.Selected += LoadGameMenuSelected;
            MenuItems.Add(loadGameMenu);

            var creditsMenu = new MenuItem("Credits")
            {
                Description = "Start a new game",
                Font = CommonAssets.StandardFont
            };
            var creditsMenuPosX = (int)(Constants.CanvasWidth / 2 - CommonAssets.StandardFont.MeasureString(creditsMenu.Text).X / 2);
            creditsMenu.Position = new Vector2(creditsMenuPosX, 115);
            creditsMenu.Selected += CreditsMenuSelected;
            MenuItems.Add(creditsMenu);

            var exitGameMenu = new MenuItem("Exit Game")
            {
                Description = "Quit the game",
                Font = CommonAssets.StandardFont
            };
            var exitGameMenuPosX = (int)(Constants.CanvasWidth / 2 - CommonAssets.StandardFont.MeasureString(exitGameMenu.Text).X / 2);
            exitGameMenu.Position = new Vector2(exitGameMenuPosX, 130);
            exitGameMenu.Selected += OnCancel;
            MenuItems.Add(exitGameMenu);
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
