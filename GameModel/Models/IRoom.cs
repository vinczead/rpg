using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models
{
    public interface IRoom : IGameObject
    {
        string Name { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Depth { get; set; }
        Texture2D Texture { get; set; }

        Dictionary<string, IGameWorldObject> GameWorldObjects { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Update(GameTime gameTime);

        void InsertGameWorldObject(IGameWorldObject gameWorldObject);

        void RemoveGameWorldObject(IGameWorldObject gameWorldObject);
    }
}
