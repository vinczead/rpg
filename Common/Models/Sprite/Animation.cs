using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{
    public class Animation
    {
        public string Id { get; set; }
        public List<Frame> Frames { get; set; } = new List<Frame>();
        public bool IsLooping { get; set; }

        public double RoundDuration { get => Frames.Sum(frame => frame.TimeSpan.TotalMilliseconds); } //todo: should be computed when building world

        public Frame FrameAt(TimeSpan animationTime)
        {
            if (Frames == null || Frames.Count == 0)
                return Frame.Empty;

            TimeSpan timeSum = TimeSpan.Zero;
            int i = 0;

            while (timeSum < animationTime && (i != Frames.Count - 1 || IsLooping))
            {
                timeSum += Frames[i].TimeSpan;

                i++;
                if (i == Frames.Count && IsLooping)
                {
                    i = 0;
                }

            }

            return Frames[i];
        }

        public static Animation Empty
        {
            get => new Animation()
            {
                Id = "FallbackAnimation",
                IsLooping = false,
                Frames = new List<Frame>()
                {
                    new Frame()
                    {
                        TimeSpan = TimeSpan.FromSeconds(1),
                        Source = new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32)
                    }
                }
            };
        }
    }
}
