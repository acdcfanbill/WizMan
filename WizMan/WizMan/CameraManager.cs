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
    public class CameraManager : Microsoft.Xna.Framework.GameComponent
    {
        public Camera camera;
        Viewport defaultViewport;

        public Vector2 parallaxFarthestBackground;
        public Vector2 parallaxBackground;
        public Vector2 parallaxMidground;
        public Vector2 parallaxForeground;
        

        public CameraManager(Game game, Viewport defaultViewport)
            : base(game)
        {
            this.defaultViewport = defaultViewport;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Camera initialization
            parallaxFarthestBackground = new Vector2(0.1f);
            parallaxBackground = new Vector2(0.5f);
            parallaxMidground = new Vector2(0.75f);
            parallaxForeground = new Vector2(1.0f);

            camera = new Camera(defaultViewport);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        { 
            //eventially do more sophisticated place checking here.
            //for now, we will just have hte camera dutifly follow the player
            Vector2 playerPosition = Game1.spriteManager.getPlayerPosition();

            //adjust the camera position
            //eventially we will want to adjust it right/left for running right and left
            //now i'm just centering him a bit more, also you will want to see more above
            //than below

            playerPosition.Y -= 125;
            playerPosition.X += 75;
            camera.LookAt(playerPosition, defaultViewport);
            base.Update(gameTime);
        }

        //not sure why I thought it should be drawable
        //public override void Draw(GameTime gameTime)
        //{

        //    base.Draw(gameTime);
        //}
    }
}
