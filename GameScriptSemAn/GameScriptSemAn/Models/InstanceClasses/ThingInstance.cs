using GameScript.Models.BaseClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameScript.Models.InstanceClasses
{
    public abstract class ThingInstance : GameObjectInstance
    {
        public Region Region { get; set; }
        public Vector2 Position { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw((Base as Thing).Texture, Position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            Executer.ExecuteRunBlock(this, "WhenInGame");
        }
    }
}