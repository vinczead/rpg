using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgEngine.Utility
{
    //Source: https://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second
    public class FpsCounter
    {
        public FpsCounter()
        {
        }

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 25;

        private Queue<float> sampleBuffer = new Queue<float>();

        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                sampleBuffer.Dequeue();
                AverageFramesPerSecond = sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}
