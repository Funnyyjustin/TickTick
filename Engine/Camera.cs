using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Camera : GameObject
    {
        private GraphicsDevice graphicsDevice;
        public CameraAnimation Animation { get; set; }
        public Point CameraViewPortSize { get; set; }
        public Rectangle CameraLimits { get; set; }
        public Matrix TranslationMatrix { get; private set; }
        public Matrix ScalingMatrix { get; private set; }
        private GameObject target;
        private float followingStrength;
        public bool IsFollowing { get; set; }

        public Camera(Point cameraViewPortSize, Rectangle cameraLimits, GraphicsDevice graphicsDevice, GameObject target = null, bool isFollowing = false, float followingStrength = 1f)
        {
            this.target = target;
            IsFollowing = isFollowing;
            this.followingStrength = followingStrength;
            CameraViewPortSize = cameraViewPortSize;
            CameraLimits = cameraLimits;
            TranslationMatrix = Matrix.Identity;
            this.graphicsDevice = graphicsDevice;
            Animation = new CameraAnimation(this);
            Zoom = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsFollowing && target != null)
            {
                // set the camera velocity, depending on the distance from the target to center of the camera
                Vector2 cameraCenter = GlobalPosition + CameraViewPortSize.ToVector2() / 2;
                float velocityMultiplier = MathF.Pow(Vector2.Distance(target.GlobalPosition, cameraCenter), followingStrength);
                Vector2 deltaPosition = target.GlobalPosition - cameraCenter;
                velocity = deltaPosition.Normalized() * velocityMultiplier;
            }
            Animation.Update(gameTime);

            Vector2 previousPosition = LocalPosition;
            base.Update(gameTime);
            if (previousPosition == LocalPosition) { velocity = Vector2.Zero; }
        }

        //Gets or overwrites the LocalPosition property, so it sets the LocalPosition clamped to the CameraLimits Rectangle. Also updates the TranslationMatrix.
        public override Vector2 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = Extensions.PositionClampedToRectangle(value, CameraLimits);

                TranslationMatrix = Matrix.CreateTranslation(
                    -GlobalPosition.X, 
                    -GlobalPosition.Y, 0
                    );
            }
        }

        //Gets or sets the zoom of the camera. Also updates the ScalingMatrix.
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                ScalingMatrix = Matrix.CreateScale(
                    (float)graphicsDevice.Viewport.Width / CameraViewPortSize.X * zoom,
                    (float)graphicsDevice.Viewport.Height / CameraViewPortSize.Y * zoom, 1
                    );
            }
        }
        private float zoom;


        public Vector2 CenteredCameraPosition(Vector2 position)
        {
            return position - CameraViewPortSize.ToVector2() / 2;
        }
    }
}
