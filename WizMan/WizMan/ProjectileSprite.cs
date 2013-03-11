using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace WizMan
{
    public class ProjectileSprite : Sprite
    {
        private Texture2D _texture;
        Game1.Power projType;
        Vector2 drction;
        float timeAlive;
        public bool alive;
        int frameOffset;
        int powerOffset;
        int msSinceNewFrame = 0;
        int msPerFrame = 250; //new frame ever quarter second
        

        public ProjectileSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, Vector2 direction, Game1.Power projType)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed)
        {
            this._texture = textureImage;
            //direction is passed in as a positive or negative X amount so we know if the
            // projectile is moving left or right
            this.drction = direction;
            this.projType = projType;
            timeAlive = 0;
            alive = true;
            frameOffset = 0;
            if (projType == Game1.Power.Fire)
                powerOffset = 0;
            if (projType == Game1.Power.Ice)
                powerOffset = 2;
            if (projType == Game1.Power.Wind)
                powerOffset = 4;
            if (projType == Game1.Power.Shock)
                powerOffset = 6;

            if (direction.X < 0) //we are going left, need to add one to the powerOffset
                powerOffset++;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeAlive += gameTime.ElapsedGameTime.Milliseconds;
            if (timeAlive > 10000)
                alive = false;
            //gameTime.ElapsedGameTime.TotalMilliseconds.
            position += drction * speed;
            base.Update(gameTime, clientBounds);
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isAlive())
            {
                msSinceNewFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceNewFrame > msPerFrame)
                {
                    msSinceNewFrame = 0;
                    frameOffset++;
                    if (frameOffset > 1)
                        frameOffset = 0;
                }

                spriteBatch.Draw(_texture, position, new Rectangle(frameOffset * frameSize.X, powerOffset * frameSize.Y,
                frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            }
                //base.Draw(gameTime, spriteBatch);

            //if not alive, don't draw
        }

        public override Vector2 direction
        {
            get 
            {
                return direction;
            }
        }

        public bool isAlive()
        {
            return alive;
        }
       
    }

    
}