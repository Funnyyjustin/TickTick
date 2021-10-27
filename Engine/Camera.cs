using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class Camera : ExtendedGame
    {
        public Rectangle WindowBox
        {
            get
            {
                Rectangle windowBox = new Rectangle(0, 0, 1024, 768);
                return windowBox;
            }
        }
    }
}
