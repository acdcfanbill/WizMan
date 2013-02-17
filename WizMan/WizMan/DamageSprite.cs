using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizMan
{
    class DamageSprite : Sprite
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Point _frameSize;
        private int damage;

        public DamageSprite(Texture2D textureImage, int damage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed) 
        {
            this._texture = textureImage;
            this._position = position;
            this._frameSize = frameSize;
            this.damage = damage;
        }
        public DamageSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
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
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y, (int)position.X + frameSize.X, (int)position.Y + frameSize.Y), Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        public int getDamage()
        {
            return damage;
        }
    }
}
