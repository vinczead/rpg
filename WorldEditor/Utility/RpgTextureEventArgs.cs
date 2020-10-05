using System;
using System.Collections.Generic;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.Utility
{
    public class RpgTextureEventArgs : EventArgs
    {
        public RpgTextureEventArgs(RpgTexture rpgTexture)
        {
            this.Texture = rpgTexture;
        }

        public RpgTexture Texture { get; set; }
    }
}
