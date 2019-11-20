using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameModel.Models
{
    public abstract class GameObject
    {
        public string Id { get; set; }
        public GameObjectBase Base { get; set; }
        public Room Room { get; set; }
        public Vector2 Position { get; set; }

        public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Base.Texture, Position, Color.White); 
        }
    }
}