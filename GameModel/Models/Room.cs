using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.Models
{
    public class Room
    {
        public Room()
        {
            GameObjects = new Dictionary<string, GameObject>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public Texture2D Texture { get; set; }

        public World World { get; set; }
        public Dictionary<string, GameObject> GameObjects { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(0, 0, Width, Height), Color.White);

            foreach (var gameObject in GameObjects)
            {
                gameObject.Value.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in GameObjects)
            {
                gameObject.Value.Update(gameTime);
            }
        }

        public void InsertGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject.Id, gameObject);
            gameObject.Room = this;
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject.Id);
            gameObject.Room = null;
        }
    }
}