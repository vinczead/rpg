using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Frame
    {
        public Rectangle Source { get; set; }
        public TimeSpan TimeSpan { get; set; }

        public static Frame Empty
        {
            get
            {
                return new Frame()
                {
                    Source = new Rectangle(0, 0, 0, 0),
                    TimeSpan = TimeSpan.Zero
                };
            }
        }
    }
}
