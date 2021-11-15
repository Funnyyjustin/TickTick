using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class CameraAnimation
    {
        Camera camera;

        //Queues that store the Zoom and Move animation objects
        Queue<Zoom> targetZoomValues = new Queue<Zoom>();
        Queue<Move> targetMoveValues = new Queue<Move>();

        public CameraAnimation(Camera camera){
            this.camera = camera;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            HandleZoom(deltaTime);
            HandleMove(deltaTime);
        }

        //queue a zoom animation for this camera
        public void AddZoomAnimation(float duration, float startZoom, float targetZoom)
        {
            if (duration == 0) return;
            targetZoomValues.Enqueue(new Zoom(duration, startZoom, targetZoom));
        }

        //if there is a zoom animation, go to the next animation step
        public void HandleZoom(float deltaTime){
            if (targetZoomValues.Count == 0) return;
            Zoom currentZoomAnimation = targetZoomValues.Peek();
            if (!currentZoomAnimation.TryNextTimeStep(deltaTime))
                targetZoomValues.Dequeue();
            camera.Zoom = currentZoomAnimation.GetCurrentZoom();
        }

        //queue a move animation for this camera
        public void AddMoveAnimation(float duration, Vector2 startMove, Vector2 targetMove)
        {
            if (duration == 0) return;
            targetMoveValues.Enqueue(new Move(duration, startMove, targetMove));
        }

        //if there is a move animation, go to the next animation step
        public void HandleMove(float deltaTime)
        {
            if (targetMoveValues.Count == 0) return;
            camera.velocity = Vector2.Zero;
            Move currentMoveAnimation = targetMoveValues.Peek();
            if (!currentMoveAnimation.TryNextTimeStep(deltaTime))
                targetMoveValues.Dequeue();
            camera.LocalPosition = currentMoveAnimation.GetCurrentMove();
        }

        private class Zoom : Timer
        {
            public float StartZoom { get; set; }
            public float TargetZoom { get; set; }

            public Zoom(float duration, float startZoom, float targetZoom) : base(duration)
            {
                StartZoom = startZoom;
                TargetZoom = targetZoom;
            }

            public float GetCurrentZoom()
            {
                //calculate the interpolation between StartZoom and TargetZoom, depending on the time that has passed since the start of this animation.
                return Extensions.Lerp(StartZoom, TargetZoom, TimePassed / Duration);
            }
        }

        private class Move : Timer
        {
            public Vector2 StartMove { get; set; }
            public Vector2 TargetMove { get; set; }

            public Move(float duration, Vector2 startMove, Vector2 targetMove) : base(duration)
            {
                Duration = duration;
                StartMove = startMove;
                TargetMove = targetMove;
                TimePassed = 0;
            }

            public Vector2 GetCurrentMove()
            {
                //calculate the interpolation between StartMove and TargetMove, depending on the time that has passed since the start of this animation.
                return Extensions.Lerp(StartMove, TargetMove, TimePassed / Duration);
            }
        }

    }
}
