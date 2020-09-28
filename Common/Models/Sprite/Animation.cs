using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Animation
    {
        public List<Frame> Frames { get; set; } = new List<Frame>();

        public bool IsLooping { get; set; }

        public Frame FrameAt(TimeSpan animationTime)
        {
            if (Frames == null || Frames.Count == 0)
                return Frame.Empty;

            TimeSpan currentTime = Frames[0].TimeSpan;
            int i = 1;
            while (currentTime < animationTime)
            {
                currentTime += Frames[i].TimeSpan;
                i++;
                if (i == Frames.Count)
                {
                    if (IsLooping)
                        i = 0;
                    else
                        break;
                }
            }

            return Frames[i - 1];
        }
    }
}
