using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<Screen> screens  = new List<Screen>();
        private readonly List<Screen> screensToUpdate = new List<Screen>();

        public Screen[] Screens { get { return screens.ToArray(); } }

        public SpriteBatch SpriteBatch { get; private set; }

        private bool isInitialized;

        public ScreenManager(Game game) : base(game)
        {

        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var screen in screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in screens)
            {
                screen.UnloadContent();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;

            if (isInitialized)
                screen.LoadContent();

            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            if (isInitialized)
                screen.UnloadContent();

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            int i = screens.Count -1;
            while (i > 0 && screens[i].IsOverlay)
                i--;

            for (; i <= screens.Count-1; i++)
            {
                screens[i].Draw(gameTime);
            }
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            screensToUpdate.Clear();
            screensToUpdate.AddRange(screens);

            bool inputAlredyHandled = false;

            while(screensToUpdate.Count > 0)
            {
                var screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime);

                if(!inputAlredyHandled)
                {
                    screen.HandleInput();

                    inputAlredyHandled = true;
                }

                if (screen.BlockScreenUpdatesBelow)
                    break;
            }

        }
    }
}
