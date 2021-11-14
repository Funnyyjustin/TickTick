using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public static class TimedActionManager
    {
        public static List<TimedAction> timedActions = new List<TimedAction>();

        public static void Add(TimedAction timedAction){
            timedActions.Add(timedAction);
        }

        public static void Remove(TimedAction timedAction)
        {
            timedActions.Remove(timedAction);
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < timedActions.Count; i++)
            {
                timedActions[i].Update(gameTime);
            }
        }
    }
}
