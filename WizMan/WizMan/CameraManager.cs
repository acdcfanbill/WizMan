using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WizMan
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CameraManager : Microsoft.Xna.Framework.GameComponent
    {
        public Camera camera;
        Viewport defaultViewport;

        public Vector2 parallaxFarthestBackground;
        public Vector2 parallaxBackground;
        public Vector2 parallaxMidground;
        public Vector2 parallaxForeground;
        public Vector2 lookPosition;
        Vector2 oldPlayerPosition;
        Vector2 playerSize;
        Vector2 screenSize;
        Vector2 screenCenter;
        bool goingRight;
        bool goingLeft;
        bool switchDirection = false;
        bool alreadySwitch = false;
        

        public CameraManager(Game game, Viewport defaultViewport)
            : base(game)
        {
            this.defaultViewport = defaultViewport;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Camera initialization
            camera = new Camera(defaultViewport);
            parallaxFarthestBackground = new Vector2(0.25f);
            parallaxBackground = new Vector2(0.5f);
            parallaxMidground = new Vector2(0.75f);
            parallaxForeground = new Vector2(1.0f);

            //initialize movement directions
            goingRight = true;
            goingLeft = false;

            //get the player and screen info once
            playerSize = Game1.spriteManager.player.getSize();
            screenSize = Game1.screenSize;

            //adjust camera position for good first view
            camera.LookAt(new Vector2(0,-100), defaultViewport);

            lookPosition = Vector2.Zero;
            lookPosition.Y = camera.Position.Y + screenSize.Y - playerSize.Y - 50;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Vector2 playerPosition = Game1.spriteManager.getPlayerPosition();
            screenCenter = camera.Position + (screenSize / 2);

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                if (!alreadySwitch)
                    switchDirection = true;
            }
            else
            {
                switchDirection = false;
                alreadySwitch = false;
            }

            if (switchDirection && !alreadySwitch)
            {
                switchDirection = false;
                switchDirections(lookPosition);
                alreadySwitch = true;
            }
            //if we arn't switching directions, just follow based on the deadzone in 
            //cameraAdjustment
            if(!switchDirection)
                cameraAdjustment(playerPosition);

            oldPlayerPosition = playerPosition;
            camera.LookAt(lookPosition, defaultViewport);
            base.Update(gameTime);
        }

        //not sure why I thought it should be drawable
        //public override void Draw(GameTime gameTime)
        //{

        //    base.Draw(gameTime);
        //}

        public void cameraAdjustment(Vector2 playerPosition)
        {
            ///
            /// this is to adjust the camera left and right according to the players movement
            /// this depends on if the player is moving right or left.
            /// my thinking is that each level will be 'mostly right' or 'mostly left' and we
            /// will switch the direction with the cameramanager function only at the level
            /// start because if you switch it when the player isn't centered it currently
            /// looks funny.  that is, it jumps the camera to the opposite side
            ///
            #region Going Left And Right
            if (goingRight)
            {
                if (playerPosition.X < camera.Position.X + playerSize.X)
                {
                    lookPosition.X += playerPosition.X - oldPlayerPosition.X;
                }
                if (playerPosition.X > screenCenter.X)
                {
                    lookPosition.X += playerPosition.X - oldPlayerPosition.X;
                }
            }
            if (goingLeft)
            {
                if (playerPosition.X < screenCenter.X)
                {
                    lookPosition.X += playerPosition.X - oldPlayerPosition.X;
                }
                if (playerPosition.X > camera.Position.X + (screenSize.X - playerSize.X*2))
                {
                    lookPosition.X += playerPosition.X - oldPlayerPosition.X;
                }
            }
            #endregion
            ///
            ///Adjusts the camera's up and down movement the -50 is just an offset to keep him
            ///somewhat above the screen.  we can change this or possibly make it adjustable
            ///
            #region Going Up and Down
            if (playerPosition.Y < camera.Position.Y)
                lookPosition.Y += playerPosition.Y - oldPlayerPosition.Y;
            if (playerPosition.Y > camera.Position.Y + screenSize.Y - playerSize.Y - 50)
                lookPosition.Y += playerPosition.Y - oldPlayerPosition.Y;
            #endregion
        }
        public void switchDirections(Vector2 newLookPos)
        {
            //if you switch the directions you MUST MAKE SURE THE CAMERA IS CENTERED on the
            //the players X coordinate first otherwise it will goof up the following.
            //to simplify this, I've just moved the camera's look position to the center X
            //coordt whenever we run this.
            Vector2 playerPosition = Game1.spriteManager.getPlayerPosition();

            if (goingRight)
            {
                if (playerPosition.X < screenCenter.X)
                {
                    newLookPos.X = playerPosition.X;
                    lookPosition = newLookPos;
                }
                goingRight = false;
                goingLeft = true;
            }
            else
            {
                if (playerPosition.X > screenCenter.X)
                {
                    newLookPos.X = playerPosition.X;
                    lookPosition = newLookPos;
                }
                goingRight = true;
                goingLeft = false;
            }
        }
    }
}
