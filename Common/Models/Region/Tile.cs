using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.Models
{
    public class Tile
    {
        public string Id { get; set; }
        public bool IsWalkable { get; set; }
        [JsonIgnore]
        public SpriteModel Model { get; set; }
        public string SpriteModelId { get; set; }

    }
}
