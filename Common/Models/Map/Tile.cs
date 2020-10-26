using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Tile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsWalkable { get; set; }
        public SpriteModel Model { get; set; }
        public string SpriteModelId { get; set; }

    }
}
