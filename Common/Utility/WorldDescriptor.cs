using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public class WorldDescriptor
    {
        public Dictionary<string, string> Textures { get; set; }
        public Dictionary<string, string> SpriteModels { get; set; }
        public Dictionary<string, string> Scripts { get; set; }
        public Dictionary<string, string> Maps { get; set; }
        public Dictionary<string, string> Tiles { get; set; }
    }
}
