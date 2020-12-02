using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEngine.Screens
{
    public abstract class Screen
    {
        public bool IsOverlay { get; set; } = false;

        public bool BlockScreenUpdatesBelow { get; set; } = true;

        public ScreenManager ScreenManager { get; set; }

        public event EventHandler LoadedContent;

        public virtual void LoadContent() {
            LoadedContent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void HandleInput() { }

        public virtual void Draw(GameTime gameTime) { }
    }
}
