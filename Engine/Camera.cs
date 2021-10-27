using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class Camera : ExtendedGame
    {
        public Point WindowBox
        {
            get
            {
                Point windowBox = windowSize;
                return windowBox;
            }
        }
    }
}
