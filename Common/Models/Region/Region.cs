using Common.Utility;
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

        public List<ThingInstance> instances = new List<ThingInstance>();

        private List<ThingInstance> instancesToDelete = new List<ThingInstance>();
        private List<ThingInstance> instancesToAdd = new List<ThingInstance>();

        public int Width { get; set; }
        public int Height { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Tile[][] Tiles { get; set; }

        public TimeSpan AnimationTime { get; set; }

        public void Update(GameTime gameTime)
        {
            AnimationTime += gameTime.ElapsedGameTime;
            foreach (var thing in instances)
                thing.Update(gameTime);

            foreach (var thing in instancesToDelete)
            {
                instances.Remove(thing);
                thing.Region = null;
            }

            foreach (var thing in instancesToAdd)
                instances.Add(thing);
            instances.Sort(new InstanceCoordinateComparer());

            instancesToDelete.Clear();
            instancesToAdd.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tilePosition = new Vector2(x * TileWidth, y * TileHeight);
                    var spriteModel = Tiles[y][x].Model;

                    spriteBatch.Draw(spriteModel.SpriteSheet.Value, tilePosition, spriteModel["Idle"].FrameAt(AnimationTime).Source, Color.White);
                }
            }
            foreach (var instance in instances)
            {
                instance.Draw(spriteBatch);
            }
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
