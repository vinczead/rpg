using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgEngine.Utility
{
    public static class Constants
    {
        public static readonly int CanvasWidth = 320;
        public static readonly int CanvasHeight = 200;
        public static Vector2 Canvas { get => new Vector2(CanvasWidth, CanvasHeight); }
    }
}
