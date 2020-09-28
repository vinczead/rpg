using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldEditor.Views
{
    public class WorldEditorGame : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        public SpriteFont StandardFont { get; set; }
        private SpriteBatch spriteBatch;


        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            // content loading now possible
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            StandardFont = Content.Load<SpriteFont>("Fonts/StandardFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();
        }

        protected override void Draw(GameTime time)
        {
            var wpfRenderTarget = (RenderTarget2D)GraphicsDevice.GetRenderTargets()[0].RenderTarget;

            SpriteBatch targetBatch = new SpriteBatch(GraphicsDevice);
            RenderTarget2D target = new RenderTarget2D(GraphicsDevice, 320, 200);
            GraphicsDevice.SetRenderTarget(target);

            targetBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            targetBatch.DrawString(StandardFont, "Hello World Editor from MonoGame", new Vector2(10, 20), Color.Yellow);
            targetBatch.End();

            GraphicsDevice.SetRenderTarget(wpfRenderTarget);

            targetBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            targetBatch.Draw(target, new Rectangle(0, 0, _graphicsDeviceManager.GraphicsDevice.Viewport.Width, _graphicsDeviceManager.GraphicsDevice.Viewport.Height), Color.White);
            targetBatch.End();
        }
    }
}
