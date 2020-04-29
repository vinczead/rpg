using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameScript.Models
{
    public class Region
    {
        public Region()
        {
            Things = new Dictionary<string, ThingInstance>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; set; }

        public GameModel GameModel { get; set; }
        public Dictionary<string, ThingInstance> Things { get; set; }

        private List<ThingInstance> thingsToRemove = new List<ThingInstance>();

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(0, 0, Width, Height), Color.White);

            foreach (var gameObject in Things)
            {
                gameObject.Value.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in Things.Values)
            {
                gameObject.Update(gameTime);
            }
            foreach (var gameObject in thingsToRemove)
            {
                Things.Remove(gameObject.Id);
            }
            thingsToRemove.Clear();
        }

        public void InsertThing(ThingInstance thingInstance)
        {
            Things.Add(thingInstance.Id, thingInstance);
            thingInstance.Region = this;
        }

        public void RemoveThing(ThingInstance thingInstance)
        {
            thingsToRemove.Add(thingInstance);
            thingInstance.Region = null;
        }
    }
}