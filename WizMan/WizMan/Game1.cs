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
        SpriteBatch spriteBatch;

        //Scrolling Background
        SpriteBatch scrBgBatch;
        Texture2D panobkg;
        Rectangle bkg;
        Vector2 bkgpos;
        Vector2 bkgorigin;
        Rectangle mainFrame;

        //Camera Background
        Vector2 parallax;
        Vector2 parallax2;
        Vector2 parallax3;

        //Various managers
        SpriteManager spriteManager;
        Camera camera;

        //Misc
        KeyboardState keyboardState;

        //Game States
        enum GameState { MainMenu, InGame, GameOver }
        GameState currentGameState = GameState.MainMenu;

        //Main Menu Text
        SpriteFont titleFont;
        SpriteFont menuFont;
        Color mColor;
        string title;
        List<String> menuOps;
        int sel;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1024;
            //graphics.PreferredBackBufferHeight = 768;
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
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;


            //Scrolling Background Initialization
            bkgpos.X = 0;
            bkgpos.Y = 0;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            bkg = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            bkgorigin.X = 0;
            bkgorigin.Y = 0;
            graphics.ApplyChanges();

            //Camera initialization
            parallax = new Vector2(0.75f);
            parallax2 = new Vector2(0.5f);
            parallax3 = new Vector2(0.66f);

            //menu
            mColor = Color.WhiteSmoke;
            title = "WIZ MAN";
            menuOps = new List<string>();
            menuOps.Add("Start New Game");
            menuOps.Add("Instructions");
            menuOps.Add("Exit Game");
            sel = 0;

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
            scrBgBatch = new SpriteBatch(GraphicsDevice);

            //Background Image
            panobkg = Content.Load<Texture2D>("bgusd");

            //Text For menu 
            titleFont = Content.Load<SpriteFont>("SpriteFont1");
            menuFont = Content.Load<SpriteFont>("mFont");

            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            camera = new Camera(GraphicsDevice.Viewport);
            camera.ZoomLevel(0.5f);

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
            keyboardState = Keyboard.GetState();
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    Boolean notPressed = true;
                    if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                    {
                        if (sel == 2)
                        {
                            sel = 0;
                        }
                        else sel++;
                    }
                    if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                    {
                        if (sel == 0)
                        {
                            sel = 2;
                        }
                        else sel--;
                    }
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (sel == 0) currentGameState = GameState.InGame;
                        if (sel == 1) currentGameState = GameState.GameOver;
                        if (sel == 2) currentGameState = GameState.GameOver;
                    }

                    break;
                case GameState.InGame:
                    spriteManager.Enabled = true;
                    spriteManager.Visible = true;


                    int speed = 10;
                    if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                        camera.Move(new Vector2(-speed, 0));
                    if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                        camera.Move(new Vector2(speed, 0));
                    break;
                case GameState.GameOver:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    this.Exit();
                    break;
            }

            // Allows the game to exit
            // Escape works. Maybe later we make it a fancy menu, but only if it's fancy.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                 || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
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
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(titleFont, title,
                                            new Vector2((Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) - 50), Color.Gold);
                    int c = 0;
                    for (int i = 0; i <= 2; i++)
                    {
                        if (sel == i) mColor = Color.Red;
                        spriteBatch.DrawString(menuFont, menuOps[i],
                                            new Vector2((Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) + c), mColor);
                        mColor = Color.WhiteSmoke;
                        c += 20;
                    }
                    spriteBatch.End();

                    break;
                case GameState.InGame:
                    //draw the background first, with the slowest parallax
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax2));
                    spriteBatch.Draw(panobkg, new Vector2(-panobkg.Width / 2.0f, -panobkg.Height / 2.0f), Color.White);
                    spriteBatch.End();

                    break;
                case GameState.GameOver:

                    break;
            }


            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
