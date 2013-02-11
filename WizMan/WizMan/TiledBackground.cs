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
    public class TiledBackground : IBackground
    {
        private readonly Texture2D _texture;
        public int _horizontalTileCount;
        public int _verticalTileCount;
        public Vector2 _startCoord;

        public TiledBackground(Texture2D texture, int enviromentWidth, int enviromentHeigth, Vector2 start)
        {
            _texture = texture;
            _horizontalTileCount = (int)(Math.Round((Double)enviromentWidth / _texture.Width) + 1);
            _verticalTileCount = (int)(Math.Round((Double)enviromentHeigth / _texture.Height) + 1);

            _startCoord = start;
        }

        public void Update(Rectangle _cameraRectangle)
        {
            _startCoord.X = ((_cameraRectangle.X / _texture.Width) * _texture.Width) - _cameraRectangle.X;
            _startCoord.Y = ((_cameraRectangle.Y / _texture.Height) * _texture.Height) - _cameraRectangle.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _horizontalTileCount; i++)
            {
                for (int j = 0; j < _verticalTileCount; j++)
                {
                    spriteBatch.Draw(_texture,
                    new Rectangle(
                    (int)_startCoord.X + (i * _texture.Width),
                    (int)_startCoord.Y + (j * _texture.Height),
                    _texture.Width, _texture.Height),
                    Color.White);
                }
            }
        }

        public Texture2D getTexture()
        {
            return _texture;
        }
    }
}
