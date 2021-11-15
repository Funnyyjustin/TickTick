using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    //Create a Timer that gives information about whether a certain amount of time has passed.
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
