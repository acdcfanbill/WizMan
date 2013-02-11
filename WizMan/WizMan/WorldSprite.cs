using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/*This class is for environmental objects that we will treat as sprites such that they will have
 collision information. If we think of them as sprites, we ought to be able to have things like elevators
 or horizontally moving platforms to do some neat gameplay things. I think there might be quite a few calls
 to generate various kinds of sprites here as we go forward: building the initial level by hand seems to be the
 option open to us to ensure that we get things just right since we are working with, eventually, powers that
 will modify our environment. We will, likely, treat the modifiable environment objects as sprites as well, but put
 them in a different class.*/
namespace WizMan
{
    class WorldSprite: Sprite
    {
        public WorldSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed) 
        { 
        }
        public WorldSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame) 
        { 
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Movement!
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
