using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class TimedAction : Timer
    {
        private Action timedAction;

        public TimedAction(float delay, Action action) : base(delay)
        {
            timedAction = action;
            TimedActionManager.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!TryNextTimeStep(deltaTime))
            {
                timedAction?.Invoke();
                TimedActionManager.Remove(this);
            }
        }
    }
}
