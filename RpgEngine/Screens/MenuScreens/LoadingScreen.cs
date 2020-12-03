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
    class LoadingScreen : Screen
    {
        bool firstFrameWasDrawn;
        Screen[] screensToLoad;

        private LoadingScreen(ScreenManager screenManager, params Screen[] screensToLoad)
        {
            this.screensToLoad = screensToLoad;
        }

        public static void Load(ScreenManager screenManager, params Screen[] screensToLoad)
        {
            foreach (var screen in screenManager.Screens)
                screenManager.RemoveScreen(screen);

            screenManager.AddScreen(new LoadingScreen(screenManager, screensToLoad));
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (firstFrameWasDrawn)
            {
                ScreenManager.RemoveScreen(this);

                foreach (var screen in screensToLoad)
                {
                    ScreenManager.AddScreen(screen);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            GuiHelper.DrawCenteredText(spriteBatch, Assets.StandardFont, "Loading...", new Vector2(Constants.CanvasWidth/2, Constants.CanvasHeight/2), Assets.StandardTextColor);

            firstFrameWasDrawn = true;
        }
    }
}
