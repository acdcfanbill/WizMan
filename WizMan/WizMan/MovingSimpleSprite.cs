using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace WizMan
{
    class MovingSimpleSprite : SimpleSprite
    {
        Vector2 velocity;

        public MovingSimpleSprite(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position)
        {
            this.velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
        }
    }
}
