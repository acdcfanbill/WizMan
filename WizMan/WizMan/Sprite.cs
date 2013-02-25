using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizMan
{
    public abstract class Sprite
    {
        //All of the variables associated with the sprite and sprite sheet.
        //Treating the wizard.png as a sprite sheet that is 1x1. Update logic reflects that
        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16; //60 FPS
        protected Vector2 speed;
        protected Vector2 position;

        public abstract Vector2 direction
        {
            get;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
        Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize,
                speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
        Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds) {
            //Moves the sprite sheet along with each image. This will make our
            //animation down the line much easier.
            //If it gets to the end of the sprite sheet, it goes down one row and keeps moving
            //If it exceeds the number of rectangles on the sheet, it starts over at the first one
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame) {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X) {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y) {
                        currentFrame.Y = 0;
                    }

                }
            }
        }
        //Overrideable draw method and direction method for controlling and drawing sprites
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X - collisionOffset,
                    (int)position.Y - collisionOffset,
                    frameSize.X + (collisionOffset),
                    frameSize.Y + (collisionOffset));
            }
        }

        public Vector2 getPosition()
        {
            return position;
        }
        public Vector2 getSize()
        {
            return new Vector2(textureImage.Width, textureImage.Height);
        }
        public void setPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }
    }
}

