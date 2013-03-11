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
    public class UserControlledSprite : Sprite
    {
        //need a current power, always start with Fire
        public Game1.Power currentPower = Game1.Power.Fire;

        public int health;
        int collisionOffset;
        bool jumping = false;
        bool canJump = true;
        Vector2 powerSpeed = new Vector2(20, 0);
        float jumpSpeed = 0;
        int amountAdded = 0;
        Vector2 lastPosition;
        bool stoppedJumpAnimation = false;
        bool cnt = false;

        int timeSinceLastFrame = 0;
        GameTime localGameTime;
        int millisecondsPerFrame;

        bool goLeft;
        bool goRight;

        //Constructors for User Controlled Sprites, straight from the book
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed)
        {
            lastPosition = position;
            this.collisionOffset = collisionOffset;
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position,
                frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            lastPosition = position;
            this.collisionOffset = collisionOffset;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //in case we need the game time in other places we can't pass it too.
            localGameTime = gameTime;

            position += direction;
            if (this.health < 1)
                Game1.currentGameState = Game1.GameState.GameOver;

            #region check direction change for firing projectile
            //need to see if we've changed which direction we are moving to
            //change which way we are shooting.
            if (position.X == lastPosition.X)
            {
                //not moving left or right
            }
            else
            {
                if (position.X > lastPosition.X)
                {
                    goRight = true;
                    goLeft = false;
                }
                if (position.X < lastPosition.X)
                {
                    goRight = false;
                    goLeft = true;
                }
            }
            #endregion
            if (shoot)
            {
                //Game1.spriteManager.projectileList.Add();
                if (goLeft)
                {
                    shootSound();
                    Game1.spriteManager.projectileList.Add(new ProjectileSprite(Game1.otherContent.Load<Texture2D>("textures/finalcastspritesheet"),
                        Game1.spriteManager.player.position, new Point(100, 50), 0, new Point(1,0), new Point(2, 8), powerSpeed, new Vector2(-1, 0), currentPower));
                }
                if (goRight)
                {
                    shootSound();
                    Game1.spriteManager.projectileList.Add(new ProjectileSprite(Game1.otherContent.Load<Texture2D>("textures/finalcastspritesheet"),
                        Game1.spriteManager.player.position, new Point(100, 50), 0, new Point(0, 0), new Point(2, 8), powerSpeed, new Vector2(1, 0), currentPower));
                }
            }

            selectPower();

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
        public void HandleCollision(List<Sprite> wList, Vector2 previousPosition)
        {
            Vector2 currPos = this.position;

            //backup player to previous position, because there was no collisions there
            this.position = previousPosition;

            //bools to that are set whether we can go up/down/left right
            bool noUp = false; bool noDown = false; bool noLeft = false; bool noRight = false;
            //create offsets for our advanced collsions.  the offset is the distance we just moved
            int ix = Math.Abs((int)currPos.X - (int)previousPosition.X);
            int iy = Math.Abs((int)currPos.Y - (int)previousPosition.Y);

            //create four collision rectangles, one above our player, one below our player one to the 
            //left and one tothe right.  If the upper one intersects any of hte current sprites we are hitting
            //the we know we can't move up.  Do the same for the other 3 direction.
            Rectangle down = this.collisionRect;
                down.Y+=iy;
            Rectangle up = this.collisionRect;
                up.Y-=iy;
            Rectangle left = this.collisionRect;
                left.X-=ix;
            Rectangle right = this.collisionRect;
                right.X+=ix;

            //preset our collision-fixed new position to the players current position.
            Vector2 newBottom = currPos;
            Vector2 newTop = currPos;
            Vector2 newLeft = currPos;
            Vector2 newRight = currPos;

            //check the four new collsion rectangels to all of hte possible collisions we passed in.
            //we may be running into more than one block at a time, so we may have to stop downward
            //movement and rightward movement at the same time.
            foreach (Sprite w in wList)
            {
                if (down.Intersects(w.collisionRect))
                {
                    noDown = true;
                    newBottom.Y = w.collisionRect.Top;
                }
                if (up.Intersects(w.collisionRect))
                {
                    noUp = true;
                    newTop.Y = w.collisionRect.Bottom;
                }
                if (left.Intersects(w.collisionRect))
                {
                    noLeft = true;
                    newLeft.X = w.collisionRect.Right;
                }
                if (right.Intersects(w.collisionRect))
                {
                    noRight = true;
                    newRight.X = w.collisionRect.Left;
                }
            }
            //if we've stopped upward movement, reset some stuff so we stop jumping and we can jump later on
            if (noUp)
            {
                //this will be changed if we get around to changing the movement to different enumerated states.
                this.jumpSpeed = 0;
                this.jumping = false;
                this.canJump = true;
            }

            //check to see if we need to step the player back in the x or y directions
            //edited this to make him move to the collision, and not just back to where he was.
            if(noDown && currPos.Y > previousPosition.Y)
                currPos.Y = newBottom.Y-this.frameSize.Y;//previousPosition.Y;
            else if (noUp && currPos.Y < previousPosition.Y)
            {
                currPos.Y = newTop.Y;// previousPosition.Y;
                amountAdded = 0; //this fixes a bug where if he hits his head after a jump, the next jump is short
            }
            if (noLeft && currPos.X < previousPosition.X)
                currPos.X = previousPosition.X;
                //currPos.X = newLeft.X+1;// previousPosition.X;
            if (noRight && currPos.X > previousPosition.X)
                currPos.X = previousPosition.X;
                //currPos.X = newRight.X - this.frameSize.X;// previousPosition.X;

            //reset the position, with adjustments if necessary
            setPosition(currPos);
            return;
        }

        //Regesters that the mouse was click and a projectile should be shot
        public Boolean shoot
        {
            get
            {
                Boolean fireProjectile = false;
                MouseState ms = Mouse.GetState();

                if (cnt)
                {
                    if (ms.LeftButton == ButtonState.Pressed) fireProjectile = true;
                    cnt = false;
                }
                if (ms.LeftButton == ButtonState.Released) cnt = true;
                return fireProjectile;
            }
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
                bool goRight = false;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    goRight = true;
                bool goLeft = false;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    goLeft = true;
                bool goFast = false;
                int localMsPerFrame = millisecondsPerFrame;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    goFast = true;
                    localMsPerFrame -= 75;
                }

                //adjust position for gravity
                inputDirection.Y += 2;

                //check directions
                if (goLeft)
                {
                    timeSinceLastFrame += localGameTime.ElapsedGameTime.Milliseconds;
                    if (!jumping && canJump && position.Y == lastPosition.Y)
                        Game1.audioManager.playFootSteps(localGameTime);
                    if (timeSinceLastFrame > localMsPerFrame)
                    {
                        timeSinceLastFrame = 0;
                        leftAnimation();
                    }
                    inputDirection.X -= 1;
                    if (goFast)
                    {
                        inputDirection.X -= 1f;
                    }
                }
                if (goRight)
                {
                    timeSinceLastFrame += localGameTime.ElapsedGameTime.Milliseconds;
                    if (!jumping && canJump && position.Y == lastPosition.Y)
                        Game1.audioManager.playFootSteps(localGameTime);
                    if (timeSinceLastFrame > localMsPerFrame)
                    {
                        timeSinceLastFrame = 0;
                        rightAnimation();
                    }
                    inputDirection.X += 1;
                    if (goFast)
                    {
                        inputDirection.X += 1f;
                    }
                }



                ///
                ///bill's jumping method
                ///

                //check jumping
                if (jumping)
                {
                    if (jumpSpeed == 0)
                    {
                        jumping = false;
                        amountAdded = 0;
                        Game1.spriteManager.player.jumpAnimation();
                        stoppedJumpAnimation = true;
                    }
                    if (doJump && amountAdded <= 6)
                    {
                        jumpSpeed -= 1;
                        amountAdded += 1;
                    }

                    inputDirection.Y += jumpSpeed;
                    jumpSpeed++;

                }
                //see if we can jump
                if (!canJump && position.Y < lastPosition.Y) //if you have jumped, and if you're on the way down
                {
                    canJump = true;
                    amountAdded = 0;
                }

                //see if we should jump
                if (!jumping && canJump && position.Y == lastPosition.Y) //if you can jump, and you're not moving up and down
                {
                    if (stoppedJumpAnimation == true) {
                        Game1.spriteManager.player.doneJumping();
                        stoppedJumpAnimation = false;
                    }
                    if (doJump)//if user wants to jump
                    {
                        jumping = true;
                        jumpSpeed = -6;
                        canJump = false;
                        Game1.audioManager.playJumpSound();
                    }
                }

                //need this to check and see if he is still moving up/down, if no, we allow a new jump
                lastPosition = position;

                return inputDirection * speed;
            }
        }

        /// <summary> Health getters and setters
        /// Going to need to be able to get and add or remove health for the player
        /// this will let other things get the health and add or remove it
        /// 
        /// the convention is to give a postive value (10) and then either remove or add that
        /// amount.  i.e. if you start with 100 hp, addHealth(10) = 110hp, and 
        /// removeHealth(10) = 90hp.
        /// </summary>
        /// <returns></returns>
        #region Health getters and setters
        public int getHealth()
        {
            return this.health;
        }
        public void addHealth(int health)
        {
            this.health += health;
        }
        public void removeHealth(int health)
        {
            this.health -= health;
        }
        #endregion

        //override collisionRect for hte player so he can move closer to the blocks on the
        //left/right sides.  top/bottom areleft alone.
        public override Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X - collisionOffset+22,
                    (int)position.Y - collisionOffset,
                    frameSize.X + (collisionOffset)-44,
                    frameSize.Y + (collisionOffset));
            }
        }

        public void shootSound()
        {
            switch (currentPower)
            {
                case Game1.Power.Fire:
                    Game1.audioManager.playFireSound();
                    break;
                case Game1.Power.Ice:
                    Game1.audioManager.playIceSound();
                    break;
                case Game1.Power.Wind:
                    Game1.audioManager.playWindSound();
                    break;
                case Game1.Power.Shock:
                    Game1.audioManager.playShockSound();
                    break;
            }
        }

        void selectPower()
        {
            KeyboardState kb = Keyboard.GetState();
            if(kb.IsKeyDown(Keys.D1))
                currentPower = Game1.Power.Fire;
            else if (kb.IsKeyDown(Keys.D2))
                currentPower = Game1.Power.Ice;
            else if (kb.IsKeyDown(Keys.D3))
                currentPower = Game1.Power.Wind;
            else if (kb.IsKeyDown(Keys.D4))
                currentPower = Game1.Power.Shock;

        }
    }
}
