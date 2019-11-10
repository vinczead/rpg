using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Models
{
    public abstract class GameObject
    {
        public string Id { get; set; }
        public GameObjectBase Base { get; set; }
        public Room Room { get; set; }
        public Vector2 Position { get; set; }

        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Base.Texture, Position, Color.White); 
        }
    }
}