﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Texture
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public Texture2D Value { get; set; }
        public byte[] ByteArrayValue { get; set; }
    }
}
