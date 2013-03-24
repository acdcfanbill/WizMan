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
        public UserControlledSprite player;
        Vector2 previousPosition;
        
        //Holds automated sprites. Use this for enemies later on, I suppose.
        //This was from the book. We'll have to get more specific for our functionality. Leaving it in for now.
        List<Sprite> spriteList = new List<Sprite>();


        List<Sprite> worldList = new List<Sprite>();
        List<SimpleSprite> backgrounds = new List<SimpleSprite>();
        List<DamageSprite> damageList = new List<DamageSprite>();
        List<DestructableSprite> desList = new List<DestructableSprite>();
        EndGame endSprite;

        //furthest away tiled background
        Texture2D topBackground;
        Texture2D middleBackground;
        Texture2D bottomBackground;
        FarthestBackground farthestBackground;
        Clouds clouds;

        //fires projectile if true
        //Boolean fireProjectile
        public List<ProjectileSprite> projectileList;

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
            projectileList = new List<ProjectileSprite>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loads the player's sprite
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>("finalsheet"), new Vector2 (0, 250), new Point (140, 169), 1, new Point (2, 3), new Point(5, 10),
                new Vector2(6, 6), 120);
            player.addHealth(100);

            //Function that creates all the sprites in the tower
            worldMaker();
            
            
            //list of sprites that will do damage
            damageList.Add(new DamageSprite(Game.Content.Load<Texture2D>("textures/clearPixel"),1000,new Vector2(-512,900),new Point(10000,10),2,Point.Zero,new Point(1,1),Vector2.Zero));


            //load a mid background, trying it say, 5 tiles wide, the i* 1024 comes from knowing the width of this texture.
            //also, the -512 is to start off to the side a bit so we don't see it.
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 2; j++)
                    backgrounds.Add(new SimpleSprite(Game.Content.Load<Texture2D>("textures/tempWall"), new Vector2(i*1024-512, -128+-j*1024)));
            }


            //load in the furthest tiled background
            //also putting some clouds out there
            topBackground = Game.Content.Load<Texture2D>("textures/topBackground");
            middleBackground = Game.Content.Load<Texture2D>("textures/middleBackground");
            bottomBackground = Game.Content.Load<Texture2D>("textures/bottomBackground");
            farthestBackground = new FarthestBackground(topBackground, middleBackground, bottomBackground, 10000, 10000, 600, -512);
            clouds = new Clouds(Game.Content.Load<Texture2D>("textures/clouds"), 10000, 5000, new Vector2(-512, -5000), 0.1f);



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
                //store players previous position for later use
                previousPosition = player.getPosition();
                //update the player based on the keyboard input
                player.Update(gameTime, Game.Window.ClientBounds);

                //check to see if the game or level is over
                if(player.collisionRect.Intersects(endSprite.collisionRect))
                    endSprite.handleCollision();

                //checks to see if a projectile should be shot 
                //if true adds new projectile to projectile list
                //going to do this elsewhere

                //fireProjectile = player.shoot;
                //if (fireProjectile)
                //{
                //    projectileList.Add(new ProjectileSprite(Game.Content.Load<Texture2D>("projectile"), new Vector2(player.getPosition().X + 25, player.getPosition().Y), new Point(30, 30), 1, new Point(0, 0), new Point(1, 1),
                //    new Vector2(6, 6), new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
                //}

                foreach (ProjectileSprite s in projectileList)
                {
                    if (s.isAlive())
                    {
                        s.Update(gameTime, Game.Window.ClientBounds);
                        foreach (DestructableSprite d in desList)
                        {
                            if (!d.canPassThru)
                            {
                                if (s.collisionRect.Intersects(d.collisionRect))
                                {
                                    //projectile collides with a destructable sprite
                                    //handle it and kill the projectile
                                    d.handleCollision(s);
                                    s.alive = false;
                                }
                            }
                        }
                    }
                    //if (!s.isAlive())
                    //    projectileList.Remove(s);
                }

                //remove dead projectiles
                for (int i = 0; i < projectileList.Count-1; i++)
                    if (!projectileList[i].isAlive())
                        projectileList.RemoveAt(i);

                //Update each sprite in list
                foreach (Sprite s in spriteList)
                {
                    s.Update(gameTime, Game.Window.ClientBounds);
                }
                //need a list of sprites to check collisions on
                List<Sprite> wList = new List<Sprite>();
                foreach (Sprite w in worldList)
                {
                    w.Update(gameTime, Game.Window.ClientBounds);
                    if(w.collisionRect.Intersects(player.collisionRect))
                    {
                        wList.Add(w);
                    }
                }
                //check for collisions with destructable objects
                foreach (DestructableSprite d in desList)
                {
                    d.Update(gameTime, Game.Window.ClientBounds);
                    if (!d.canPassThru && d.collisionRect.Intersects(player.collisionRect))
                    {
                        wList.Add(d);
                    }
                }
                //handle all the sprite collisions
                player.HandleCollision(wList, previousPosition);
                //remove/clean the list
                wList.Clear();

                //check for any damage sprites that affect the player
                foreach (DamageSprite w in damageList)
                {
                    w.Update(gameTime, Game.Window.ClientBounds);
                    if (w.collisionRect.Intersects(player.collisionRect))
                    {
                        player.removeHealth(w.getDamage());
                    }
                }

                //update the cloud position each frame.
                clouds.Update(Game.Window.ClientBounds);

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.currentGameState == Game1.GameState.InGame)
            {
                //draw the farthest tiled background first
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                    Game1.cameraManager.camera.GetViewMatrix(Game1.cameraManager.parallaxFarthestBackground));
                farthestBackground.Draw(spriteBatch);
                clouds.Draw(spriteBatch);
                spriteBatch.End();


                //draw the middle background next, this should be tower walls w/ see thru
                //holes in it etc.
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                    Game1.cameraManager.camera.GetViewMatrix(Game1.cameraManager.parallaxMidground));
                foreach (SimpleSprite s in backgrounds)
                    s.Draw(gameTime, spriteBatch);
                spriteBatch.End();


                //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                    Game1.cameraManager.camera.GetViewMatrix(Game1.cameraManager.parallaxForeground));
                //Draw the player
                player.Draw(gameTime, spriteBatch);
                //Draw all other sprites here, eventually
                foreach (Sprite w in worldList)
                {
                    w.Draw(gameTime, spriteBatch);
                }
                foreach (ProjectileSprite s in projectileList)
                {
                    s.Draw(gameTime, spriteBatch);
                }
                foreach (Sprite d in desList)
                {
                    d.Draw(gameTime, spriteBatch);
                }
                endSprite.Draw(gameTime,spriteBatch);
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

        //function to create whole level
        public void worldMaker()
        {
            #region longblocks
            //Long Blocks
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(0, 700),
               new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(512, 400),
               new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(1000, 700),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(2300, 550),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            //Constrained Jump Puzzle Blocks (CJP blocks)
            //Bottom part of CJP
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(3200, 550),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(4000, 550),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            //Top part of CJP
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(3400, 100),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("worldspriteplaceholder"), new Vector2(4008, 100),
                new Point(608, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            #endregion

            #region squareblocks
            //Square Blocks
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(1800, 700),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(2000, 550),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(2200, -82),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            //Elevating Puzzle Blocks
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(5100, 550),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(5300, 250),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            worldList.Add(new WorldSprite(Game.Content.Load<Texture2D>("textures/block2"), new Vector2(5000, 150),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            #endregion

            #region vines
            //First Vines (Counted by the order in which you encounter them)
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2200, 350),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2200, 242),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2200, 134),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2200, 26),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2254, 350),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2254, 242),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2254, 134),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(2254, 26),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));

            //constrained jump puzzle vines
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(4000, 442),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(3904, 208),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));

            //Elevating Puzzle Vines
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(5300, 142),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(5354, 142),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));

            //Vines before goal
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(4562, -8),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            desList.Add(new DestructableSprite(Game.Content.Load<Texture2D>("textures/vinespritesheet2"), new Vector2(4562, -116),
                new Point(54, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero, false, Game1.Power.Fire));
            #endregion
            #region goalblock
            endSprite = new EndGame(Game.Content.Load<Texture2D>("textures/powerpedestalsprite"), new Vector2(3400, -8),
                new Point(108, 108), 2, new Point(0, 0), new Point(1, 1), Vector2.Zero);
            #endregion
        }

    }
}
