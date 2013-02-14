﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizMan
{
    class UserControlledSprite: Sprite
    {
        bool jumping = false;
        bool canJump = true;
        float startY; 
        float jumpSpeed = 0;
        int amountAdded = 0;
        Vector2 lastPosition;
        
        //Controls player movement. Considering game gravity to be a part of player movement since it's
        //the other half of the jump end of things.
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                inputDirection.Y += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                    inputDirection.X += 1;
                    if (Keyboard.GetState().IsKeyDown(Keys.E)) {
                        inputDirection.X += 1f;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                    inputDirection.X -= 1;
                    if (Keyboard.GetState().IsKeyDown(Keys.E)) {
                        inputDirection.X -= 1f;
                    }
                }
                
                return inputDirection * speed;
            }
        }

        //Constructors for User Controlled Sprites, straight from the book
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed) {
                    lastPosition = position;
           }
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position,
                frameSize, collisionOffset, currentFrame, sheetSize, speed) {
                    lastPosition = position;
          }

        public override void Update(GameTime gameTime, Rectangle clientBounds) {

            bool doJump = false;
            if(Keyboard.GetState().IsKeyDown(Keys.Space))
                doJump = true;
            //The missing piece of the movement puzzle. Have to tweak numbers a bit to get him movin' like he was.
            position += direction;


            if (jumping)
            {
                if (jumpSpeed == 0)
                {
                    jumping = false;
                }
                if (doJump && amountAdded <= 14)
                {
                    jumpSpeed -= 2;
                    amountAdded += 2;
                }

                position.Y += jumpSpeed;
                jumpSpeed++;
            }

            ///
            ///bill's jumping method
            ///

            if (!canJump && position.Y < lastPosition.Y) //if you have jumped, and if you're on the way down
            {
                canJump = true;
                amountAdded = 0;
            }

            if (!jumping && canJump && position.Y == lastPosition.Y) //if you can jump, and you're not moving up and down
            {
                if (doJump)
                {
                    jumping = true;
                    jumpSpeed = -16;
                    canJump = false;
                }
            }

            //For Jumping. It very likely has to be in, or associated with, the update method of this class. I suppose we could
            //make it a seperate function somewhere and use it for potential jumping of all sprites and just have it depend on
            //what we pass it.

            //Fixed this so it ought to work regardless of whether or not the Wizard is on ground level.
            //If he isn't currently jumping, his current position is checked and then assigned to startY
            //to always be ready for him to jump. Is there a more efficient way to do this?
            //if (!jumping)
            //{
            //    startY = position.Y;
            //}
            //if (jumping)
            //{
            //    position.Y += jumpspeed;
            //    jumpspeed += 1;
            //    if (position.Y >= startY)
            //    {
            //        position.Y = startY;
            //        jumping = false;
            //    }
            //}
            //else {
            //    if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            //        jumping = true;
            //        jumpspeed = -25;
            //    }
            //}



            //if (position.X < 0) position.X = 0;
            //if (position.Y < 0) position.Y = 0;
            //commenting this out, as this holds him inside the frame
            //if (position.X > clientBounds.Width - frameSize.X) position.X = clientBounds.Width - frameSize.X;
            //if (position.Y > clientBounds.Height - frameSize.Y) position.Y = clientBounds.Height - frameSize.Y;

            lastPosition = position;
            
            position.Y += 2;

            base.Update(gameTime, clientBounds);
        }

        public void speedChange(Rectangle collisionRect) {
            //Takes the Y value of the top left corner of the rectangle and subtracts the wizard's height such that
            //his hat doesn't hang on it. It actually kind of looks like he uses his hat as a fancy zipline device if
            //you take out the -143, which begs the question: Do we need to make his hat into a fancy zipline traversal tool?
            position.Y = (float)collisionRect.Y-143;
            startY = position.Y;
        }
        
    }
}
