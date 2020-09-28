using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public class GameDescriptor
    {
        public Dictionary<string, string> Textures { get; set; }
        public Dictionary<string, string> Fonts { get; set; }
        public Dictionary<string, Color> Colors { get; set; }
    }
}
