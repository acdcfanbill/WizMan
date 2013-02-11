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
        Camera camera;
        Viewport defaultViewport;

        Vector2 parallaxBackground;
        Vector2 parallaxMidground;
        Vector2 parallaxForeground;
        

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

            camera.LookAt(Game1.spriteManager.getPlayerPosition(), defaultViewport);
            base.Update(gameTime);
        }

        //not sure why I thought it should be drawable
        //public override void Draw(GameTime gameTime)
        //{

        //    base.Draw(gameTime);
        //}
    }
}
