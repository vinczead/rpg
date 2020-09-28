using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Map
    {
        public string Id { get; set; }
        public string Name { get; set; }

        List<ThingInstance> things = new List<ThingInstance>();

        private List<ThingInstance> thingsToDelete = new List<ThingInstance>();
        private List<ThingInstance> thingsToAdd = new List<ThingInstance>();

        public int Width { get; set; }
        public int Height { get; set; }
        public string[][] Tiles { get; set; }

        public void Update(GameTime gameTime)
        {
            foreach (var thing in things)
                thing.Update(gameTime);

            foreach (var thing in thingsToDelete)
                things.Remove(thing);

            foreach (var thing in thingsToAdd)
                things.Add(thing);

            thingsToDelete.Clear();
            thingsToAdd.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void AddThing(ThingInstance thing)
        {
            thingsToAdd.Add(thing);
        }

        public void RemoveThing(ThingInstance thing)
        {
            thingsToDelete.Add(thing);
        }
     }
}
