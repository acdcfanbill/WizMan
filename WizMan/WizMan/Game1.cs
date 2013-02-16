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

    /*Joe's Update 2/1/2013:
     Worked from the book to create various manager classes to, ideally, make our lives easier
     later on. The same logic here is applicable to, I imagine, the Player class (should we need it)
     and other things like loading the background image as an asset or sounds. The file from which we
     were working was getting pretty cluttered. This was all from the book, the relevant chapters, and
     we ought to be able to quickly carry over our old work and more effectively code what we need going
     forward. I did just use the book defaults and as of about 1:30 AM I couldn't get a few things working,
     and I definitely didn't get anything added in such as the amazing jump logic that Gunnar (?) developed.
     
     If we end up not working from this and continuing to modify the other code, I feel like that's fine as well.
     I wanted the option to do things this way and figured I would do the grunt work for it.*/
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        //graphics properties
        private int preferredWidth;
        private int preferredHeight;
        public static Vector2 screenSize;

        //Scrolling Background
        //SpriteBatch scrBgBatch;
        //Texture2D panobkg;
        //Rectangle bkg;
        //Vector2 bkgpos;
        //Vector2 bkgorigin;
        //Rectangle mainFrame;

        //Various managers
        public static SpriteManager spriteManager;
        public static CameraManager cameraManager;
        public static AudioManager audioManager;
        public static Menus menu;

        //GameState info
        public enum GameState { MainMenu, PauseMenu, NewGame, InGame, GameOver }
        public static GameState currentGameState = GameState.MainMenu;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            preferredWidth = 1024;
            preferredHeight = 768;
            screenSize = new Vector2(preferredWidth, preferredHeight);

            //don't want fulscreen deving, want a windowed game.
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Creates the sprite manager and lets us use it.
            //Sprite manager handles the drawing for the player's sprite and various enemies.
            //Might also be able to use it for environmental objects such as boxes or rocks or platforms
            spriteManager = new SpriteManager(this);
            cameraManager = new CameraManager(this, GraphicsDevice.Viewport);
            audioManager = new AudioManager(this);
            menu = new Menus(this);
            Components.Add(spriteManager);
            Components.Add(cameraManager);
            Components.Add(audioManager);
            Components.Add(menu);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;


            //make sure to start off in the Main Menu
            currentGameState = GameState.MainMenu;
            base.Initialize();



        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //not sure why these spritemanager things are here, but i'm leaving them for now.
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    break;
                case GameState.NewGame:
                    //do the code to start a new game
                    currentGameState = GameState.InGame;
                    break;
                case GameState.InGame:
                    spriteManager.Enabled = true;
                    spriteManager.Visible = true;
                    break;
                case GameState.PauseMenu:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    break;
                case GameState.GameOver:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    this.Exit();
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                     break;
                case GameState.NewGame:
                     break;
                case GameState.InGame:
                    break;
                case GameState.PauseMenu:
                    break;
                case GameState.GameOver:
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
