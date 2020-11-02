using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Region
    {
        public string Id { get; set; }
        public string Name { get; set; }

        List<ThingInstance> instances = new List<ThingInstance>();

        private List<ThingInstance> instancesToDelete = new List<ThingInstance>();
        private List<ThingInstance> instancesToAdd = new List<ThingInstance>();

        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[,] Tiles { get; set; }

        public void Update(GameTime gameTime)
        {
            foreach (var thing in instances)
                thing.Update(gameTime);

            foreach (var thing in instancesToDelete) {
                instances.Remove(thing);
                thing.Region = null;
            }

            foreach (var thing in instancesToAdd)
                instances.Add(thing);

            instancesToDelete.Clear();
            instancesToAdd.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void AddInstance(ThingInstance instance)
        {
            instancesToAdd.Add(instance);
            instance.Region = this;
        }

        public void RemoveInstance(ThingInstance instance)
        {
            instancesToDelete.Add(instance);
        }
     }
}
