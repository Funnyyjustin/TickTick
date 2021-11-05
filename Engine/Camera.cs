using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Camera : GameObject
    {
        public Point CameraViewPortSize { get; set; } = new Point(1440, 825);
        public Rectangle CameraLimits { get; set; }

        //Overwrites the LocalPosition property, so it sets the LocalPosition clamped to the CameraLimits Rectangle
        public override Vector2 LocalPosition
        {
            get { return localPosition; }
            set
            {
                int rightLimit = CameraLimits.Right - CameraViewPortSize.X;
                int bottomLimit = CameraLimits.Bottom - CameraViewPortSize.Y;
                if (rightLimit < CameraLimits.Left) { rightLimit = CameraLimits.Left; }
                if (bottomLimit < CameraLimits.Top) { bottomLimit = CameraLimits.Top; }

                localPosition = new Vector2(
                    Math.Clamp(value.X, CameraLimits.Left, rightLimit),
                    Math.Clamp(value.Y, CameraLimits.Top, bottomLimit)
                    );
            }
        }
    }
}
