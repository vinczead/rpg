using GameModel.Models.InstanceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.Models
{
    public class Room : IRoom
    {
        public Room()
        {
            GameWorldObjects = new Dictionary<string, IGameWorldObject>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public Texture2D Texture { get; set; }

        public IWorld World { get; set; }
        public Dictionary<string, IGameWorldObject> GameWorldObjects { get; set; }
        public Dictionary<string, Variable> Variables { get; set; }

        private List<IGameWorldObject> gameWorldObjectsToRemove = new List<IGameWorldObject>();

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(0, 0, Width, Height), Color.White);

            foreach (var gameObject in GameWorldObjects)
            {
                gameObject.Value.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in GameWorldObjects.Values)
            {
                gameObject.Update(gameTime);
            }
            foreach (var gameObject in gameWorldObjectsToRemove)
            {
                GameWorldObjects.Remove(gameObject.Id);
            }
            gameWorldObjectsToRemove.Clear();
        }

        public void InsertGameWorldObject(IGameWorldObject gameWorldObject)
        {
            GameWorldObjects.Add(gameWorldObject.Id, gameWorldObject);
            gameWorldObject.Room = this;
        }

        public void RemoveGameWorldObject(IGameWorldObject gameWorldObject)
        {
            //GameWorldObjects.Remove(gameWorldObject.Id);
            gameWorldObjectsToRemove.Add(gameWorldObject);
            gameWorldObject.Room = null;
        }
    }
}