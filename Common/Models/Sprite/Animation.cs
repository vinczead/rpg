using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Animation
    {
        public string Id { get; set; }
        public List<Frame> Frames { get; set; } = new List<Frame>();

        public bool IsLooping { get; set; }

        public Frame FrameAt(TimeSpan animationTime)
        {
            if (Frames == null || Frames.Count == 0)
                return Frame.Empty;

            TimeSpan timeSum = TimeSpan.Zero;
            int i = 0;
            do
            {
                timeSum += Frames[i].TimeSpan;

                i++;
                if (i == Frames.Count && IsLooping)
                {
                    i = 0;
                }
            } while (timeSum < animationTime && (i == Frames.Count - 1 || IsLooping));

            return Frames[i];
        }
    }
}
