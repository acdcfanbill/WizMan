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
    class FarthestBackground : Microsoft.Xna.Framework.Game
    {
        TiledBackground topTexture;
        TiledBackground bottomTexture;
        Texture2D Middle;
        int midHeight;
        int midTop;
        int leftEdge;
        int totalWidth;

        public FarthestBackground (Texture2D top, Texture2D middle, Texture2D bottom, int totalHeight,
                                    int totalWidth, int middleMidPoint, int leftEdge)
        {
            //figure out the height of each section.  middle section will be the height of the texture.
            int topHeight = ((totalHeight - middle.Height) / 2);
            this.midHeight = middle.Height;
            int botHeight = topHeight;

            //figure out the top point of hte bottom tile and the bottom point of hte top tile
            int topBottom = middleMidPoint - (middle.Height / 2);
            midTop = topBottom;
            this.leftEdge = leftEdge;
            this.totalWidth = totalWidth;

            //figure out the top start point
            int topTop = topBottom - topHeight;

            //instantiate tiled backgrounds and set Middle texture2d
            topTexture = new TiledBackground(top, totalWidth, topHeight, new Vector2(leftEdge,topTop - (middle.Height / 2)));
            topTexture._verticalTileCount--; //for some reason, have to adjust this count
            this.Middle = middle;
            bottomTexture = new TiledBackground(bottom, totalWidth, botHeight,new Vector2(leftEdge, topBottom+(bottom.Height /2)));

        }

        public void Update()
        {
            // nothing to do here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            topTexture.Draw(spriteBatch);
            // have to draw middle tile by hand since it a big baby when passed to TiledBackground
            for(int i = 0; i < topTexture._horizontalTileCount; i++)
                spriteBatch.Draw(Middle, new Rectangle(leftEdge + (i*Middle.Width),midTop,Middle.Width,Middle.Height), Color.White);
            bottomTexture.Draw(spriteBatch);

        }
    }
}
