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
    public class EndGame : Sprite
    {
        Texture2D _texture;
        public EndGame(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed)
        {
            this._texture = textureImage;
        }


        public void handleCollision()
        {
            Game1.currentGameState = Game1.GameState.YouWin;
        }

        public override Vector2 direction
        {
            get { throw new NotImplementedException(); }
        }
    }
}
