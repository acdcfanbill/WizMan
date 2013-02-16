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
    public class Clouds : TiledBackground
    {
        //Want to be able to access properties of the texture in here
        private Texture2D __texture;

        //this is the speed at whitch the clouds float in the X direction.
        float speed;

        public Clouds(Texture2D _texture, int envWidth, int envHeight, Vector2 startPos, float speed)
            : base(_texture, envWidth, envHeight, startPos)
        {
            this.speed = speed;
            this.__texture = _texture;
        }

        public new void Update(Rectangle _cameraRectangle)
        {
            base._startCoord.X += speed;
            if (base._startCoord.X > - __texture.Width)
                base._startCoord.X = - (__texture.Width * 2);

            //don't call teh base since that kills all the movement
            //base.Update(_cameraRectangle);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}