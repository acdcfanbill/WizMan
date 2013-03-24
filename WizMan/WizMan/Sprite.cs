using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizMan
{
    public abstract class Sprite
    {
        //All of the variables associated with the sprite and sprite sheet.
        //Treating the wizard.png as a sprite sheet that is 1x1. Update logic reflects that
        Texture2D textureImage;
        protected Point frameSize;
        public Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        //int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 120; //60 FPS
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

        public virtual void Update(GameTime gameTime, Rectangle clientBounds){
            /*I made these methods such that we can call them from wherever
            so that if the wizard gets a power, we can just call the correct
             animation method rather than writing in a bunch of conditional stuff
             here. Same for jumping. I think that'll work better?*/

            //getting rid of this here, because it fucks up when other classes inherit from Sprite

            //timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            //if (timeSinceLastFrame > millisecondsPerFrame)
            //{
            //    timeSinceLastFrame = 0;
            //    if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            //        rightAnimation();
            //    }
            //    if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            //        leftAnimation();
            //    }
            //}

        }
        //Overrideable draw method and direction method for controlling and drawing sprites
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public virtual Rectangle collisionRect
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
            return new Vector2(frameSize.X, frameSize.Y);
        }
        public void setPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        public void rightAnimation(int powerOffset) {
            Game1.spriteManager.player.currentFrame.Y = powerOffset+1;
            ++Game1.spriteManager.player.currentFrame.X;
            if (Game1.spriteManager.player.currentFrame.X > 3)
            {
                Game1.spriteManager.player.currentFrame.X = 0;
            }
        }

        public void leftAnimation(int powerOffset) {
            Game1.spriteManager.player.currentFrame.Y = powerOffset+0;
            ++Game1.spriteManager.player.currentFrame.X;
            if (Game1.spriteManager.player.currentFrame.X > 3)
            {
                Game1.spriteManager.player.currentFrame.X = 0;
            }
        }

        #region JumpAnimations
        public void jumpAnimation() {
            Game1.spriteManager.player.currentFrame.X = 4;
        }
        public void doneJumping() {
            Game1.spriteManager.player.currentFrame.X = 0;
        }
        #endregion JumpAnimations
    }
}

