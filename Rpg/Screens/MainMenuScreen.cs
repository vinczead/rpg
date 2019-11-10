using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(RpgGame game) : base(game)
        {
            items.Add("Start New Game", StartGame);
            items.Add("Load Saved Game", LoadGame);
            items.Add("Quit Game", ExitGame);
        }

        private void ExitGame()
        {
            game.Exit();
        }

        private void LoadGame()
        {
            
        }

        private void StartGame()
        {
            game.AddScreen(new GameScreen(game));
        }
    }
}
