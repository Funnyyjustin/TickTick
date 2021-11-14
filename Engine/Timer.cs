using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Timer
    {
        public float Duration { get; set; }
        public float TimePassed { get; set; }

        public Timer(float duration)
        {
            Duration = duration;
            TimePassed = 0;
        }

        public bool TryNextTimeStep(float deltaTime)
        {
            TimePassed += deltaTime;
            if (TimePassed >= Duration)
            {
                TimePassed = Duration;
                return false;
            }
            return true;
        }
    }
}
