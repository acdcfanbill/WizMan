using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace WizMan
{
    public class UserControlledSprite : Sprite
    {
        bool jumping = false;
        bool canJump = true;
        //float startY;
        float jumpSpeed = 0;
        int amountAdded = 0;
        Vector2 lastPosition;

        //Constructors for User Controlled Sprites, straight from the book
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed)
        {
            lastPosition = position;
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position,
                frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            lastPosition = position;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            base.Update(gameTime, clientBounds);
        }

        public void speedChange(Rectangle collisionRect)
        {
            //Takes the Y value of the top left corner of the rectangle and subtracts the wizard's height such that
            //his hat doesn't hang on it. It actually kind of looks like he uses his hat as a fancy zipline device if
            //you take out the -143, which begs the question: Do we need to make his hat into a fancy zipline traversal tool?
            position.Y = (float)collisionRect.Y - 144;
            //startY = position.Y;
        }

        //Controls player movement. Considering game gravity to be a part of player movement since it's
        //the other half of the jump end of things.
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;


                bool doJump = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    doJump = true;
                bool goLeft = false;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    goLeft = true;
                bool goRight = false;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    goRight = true;
                bool goFast = false;
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                    goFast = true;

                //adjust position for gravity
                inputDirection.Y += 2;

                //check directions
                if (goLeft)
                {
                    inputDirection.X += 1;
                    if (goFast)
                    {
                        inputDirection.X += 1f;
                    }
                }
                if (goRight)
                {
                    inputDirection.X -= 1;
                    if (goFast)
                    {
                        inputDirection.X -= 1f;
                    }
                }

                //check jumping
                if (jumping)
                {
                    if (jumpSpeed == 0)
                    {
                        jumping = false;
                        amountAdded = 0;
                    }
                    if (doJump && amountAdded <= 6)
                    {
                        jumpSpeed -= 1;
                        amountAdded += 1;
                    }

                    inputDirection.Y += jumpSpeed;
                    jumpSpeed++;
                }

                ///
                ///bill's jumping method
                ///

                if (!canJump && position.Y < lastPosition.Y) //if you have jumped, and if you're on the way down
                {
                    canJump = true;
                    amountAdded = 0;
                    Game1.audioManager.playJumpSound();
                }

                if (!jumping && canJump && position.Y == lastPosition.Y) //if you can jump, and you're not moving up and down
                {
                    if (doJump)
                    {
                        jumping = true;
                        jumpSpeed = -6;
                        canJump = false;
                    }
                }

                //need this to check and see if he is still moving up/down, if no, we allow a new jump
                lastPosition = position;

                return inputDirection * speed;
            }
        }
    }
}
