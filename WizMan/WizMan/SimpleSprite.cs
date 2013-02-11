using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizMan
{
    class SimpleSprite
    {
        //assuming position is upper left(0,0)
        public Vector2 position;
        Texture2D texture;

        public SimpleSprite(Texture2D texture, Vector2 position)
        {
            this.position = position;
            this.texture = texture;
        }

        public int getWidth()
        {
            return texture.Width;
        }
        public int getHeight()
        {
            return texture.Height;
        }
        public Vector2 getCenter()
        {
            return new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)texture.Width, (int)texture.Height), Color.White);
        }

    }
}
