using GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rpg.Screens;
using System.Collections.Generic;

namespace Rpg
{
    public class RpgGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Screen> screens = new List<Screen>();

        public RpgGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            screens.Add(new MainMenuScreen(this));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Content = Content;
            TextureManager.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            screens[screens.Count - 1].Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(2));

            var firstIndex = screens.Count - 1;

            while (screens[firstIndex].IsOverlay)
                firstIndex--;

            for (int i = firstIndex; i < screens.Count; i++)
            {
                screens[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void AddScreen(Screen screen)
        {
            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            screens.Remove(screen);
        }
    }
}
