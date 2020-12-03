using Common.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgEngine.Screens;
using RpgEngine.Utility;

namespace RpgEngine
{
    public class RpgEngine : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;
        private ScreenManager screenManager;
        private FpsCounter fpsCounter = new FpsCounter();

        public RpgEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Role Playing Game";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            InputHandler.Initialize();
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            base.Initialize();

            screenManager.AddScreen(new MainMenuScreen());

            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(GraphicsDevice, Constants.CanvasWidth, Constants.CanvasHeight);

            Assets.LoadGameContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            fpsCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            var fps = string.Format("FPS: {0}", fpsCounter.AverageFramesPerSecond);
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(Assets.StandardFont, fps, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
