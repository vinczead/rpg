using GameModel.Models;
using GameModel.Models.BaseInterfaces;
using GameModel.Models.InstanceInterfaces;
using GameScript;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rpg.Models.InstanceClasses;
using System.Collections.Generic;

namespace Rpg.Models
{
    public abstract class GameWorldObject : GameObject, IGameWorldObject
    {
        public IGameWorldObjectBase Base { get; set; }
        public IRoom Room { get; set; }
        public Vector2 Position { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw((Base as IGameWorldObjectBase).Texture, Position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            Executer.ExecuteRunBlock(this, "WhenInGame");
        }
    }
}