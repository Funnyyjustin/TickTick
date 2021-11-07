using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class FollowingCamera : Camera
    {
        GameObject target;
        float followingStrength = 1.2f;
        public Vector2 Offset { get; set; }

        public FollowingCamera(Point cameraViewPortSize, Rectangle cameraLimits, GameObject target, Vector2 offset) : base(cameraViewPortSize, cameraLimits) {
            this.target = target;
            Offset = offset;
        }

        public override void Update(GameTime gameTime)
        {
            // set the camera velocity, depending on the distance from the target to center of the camera
            Vector2 cameraCenter = GlobalPosition + CameraViewPortSize.ToVector2() / 2;
            float velocityMultiplier = MathF.Pow(Vector2.Distance(target.GlobalPosition + Offset, cameraCenter), followingStrength);
            Vector2 deltaPosition = target.GlobalPosition + Offset - cameraCenter;
            velocity = deltaPosition.Normalized() * velocityMultiplier;
            base.Update(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
            LocalPosition = target.GlobalPosition + Offset - CameraViewPortSize.ToVector2() / 2;
        }

    }
}
