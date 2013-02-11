using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WizMan
{
   public class Camera
    {
        public Camera(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width * 2, viewport.Height);
            Zoom = 1.0f;
        }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                // Origin is before Rotation and Zoom so movement is relative to upper left but
                // zoom/rotation is around center
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void Rotate(float amount)
        {
            Rotation += amount;
            if (Rotation >= 360)
                Rotation = 0;
            if (Rotation <= -360)
                Rotation = 0;
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public void ZoomLevel(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.1f)
                Zoom = 0.1f;
        }

        public void LookAt(Vector2 position, Viewport passedViewport)
        {
            Position = position - new Vector2(passedViewport.Width / 2.0f, passedViewport.Height / 2.0f);
        }
    }
}
