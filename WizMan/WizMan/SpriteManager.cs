using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WizMan
{
    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Variables to hold various kinds of sprites. Names should be obvious.
        SpriteBatch spriteBatch;
        //Holds player's sprite
        UserControlledSprite player;
        //Holds automated sprites. Use this for enemies later on, I suppose.
        //This was from the book. We'll have to get more specific for our functionality. Leaving it in for now.
        List<Sprite> spriteList = new List<Sprite>();


        List<Sprite> worldListBackground = new List<Sprite>();
        List<Sprite> worldListMidground = new List<Sprite>();
        List<Sprite> worldListForground = new List<Sprite>();

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loads the player's sprite
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>("wizard"), new Vector2 (0, 768-150), new Point (69, 143), 1, new Point (0, 0), new Point(1, 1),
                new Vector2(6, 6));

            worldListBackground.Add(new WorldSprite(Game.Content.Load<Texture2D>("bgusd"), new Vector2 (120, 400),
               new Point (608, 108), 2, new Point (0, 0), new Point(1, 1), Vector2.Zero));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //Update player
            if (Game1.currentGameState == Game1.GameState.InGame)
            {
                player.Update(gameTime, Game.Window.ClientBounds);
                //Update each sprite in list
                foreach (Sprite s in spriteList)
                {
                    s.Update(gameTime, Game.Window.ClientBounds);
                }
                foreach (Sprite w in worldList)
                {
                    w.Update(gameTime, Game.Window.ClientBounds);
                    if (w.collisionRect.Intersects(player.collisionRect))
                    {
                        player.speedChange(w.collisionRect);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.currentGameState == Game1.GameState.InGame)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                //Draw the player
                player.Draw(gameTime, spriteBatch);
                //Draw all other sprites here, eventually
                foreach (Sprite w in worldList)
                {
                    w.Draw(gameTime, spriteBatch);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }




        ///
        ///helper functions
        ///
        ///
        public Vector2 getPlayerPosition()
        {
            return player.getPosition();
        }
    }
}
