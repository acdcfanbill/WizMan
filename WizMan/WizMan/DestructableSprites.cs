using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WizMan
{
    class DestructableSprite : Sprite
    {
        bool isAlive = true;
        public bool canPassThru = false;
        Game1.Power vunerableTo;

        public DestructableSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, bool canPassThru, Game1.Power vunerableTo)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.canPassThru = canPassThru;
            this.vunerableTo = vunerableTo;
        }
        public DestructableSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame, bool canPassThru, Game1.Power vunerableTo)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            this.canPassThru = canPassThru;
            this.vunerableTo = vunerableTo;

        }

        public override Vector2 direction
        {
            get { return speed; }
        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (!isAlive)
                currentFrame.Y = 1;
            
            base.Update(gameTime, clientBounds);
        }

        public void handleCollision(ProjectileSprite p)
        {
            if (p.projType == vunerableTo)
            {
                isAlive = false;
                canPassThru = true;
            }
        }
    }

}