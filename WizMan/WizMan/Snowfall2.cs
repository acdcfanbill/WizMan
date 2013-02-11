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
    public class Snowfall2 : TiledBackground
    {
        public Snowfall2(Texture2D _texture, int envWidth, int envHeight, Vector2 startPos, float speed)
            : base(_texture, envWidth, envHeight, startPos)
        {
            this.speed = speed;
        }
        
        float speed;

        public new void Update(Rectangle _cameraRectangle)
        {
            base._startCoord.Y+=speed;
            if (base._startCoord.Y > -1024)
                base._startCoord.Y = -2048;

            //don't call teh base since that kills all the movement
            //base.Update(_cameraRectangle);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
