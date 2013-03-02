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
    public class Menus : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Game States
        private Game1.GameState previousGameState;
        private Game1.GameState savedGameState;
        KeyboardState keyboardState;
        bool keyDown = false;
        bool moveDown = false;
        bool moveUp = false;
        
        //Main Menu Text
        SpriteBatch menuSpriteBatch;
        Texture2D deathScreen;
        Texture2D instructionScreen;
        SpriteFont titleFont;
        SpriteFont menuFont;
        Color mColor;
        string title;
        List<String> menuOps;
        List<String> pauseOps;
        List<String> gameOverOps;
        int sel;
        
        //need a scaled rectangle for the instruciton screen to display on
        Rectangle instRect;

        //three options currently
        int selMax = 2;

        public Menus(Game game)
            : base(game)
        {
            //main menu
            mColor = Color.WhiteSmoke;
            title = "WIZ MAN";
            menuOps = new List<string>();
            menuOps.Add("Start New Game");
            menuOps.Add("Instructions (currently exits)");
            menuOps.Add("Exit Game");

            //pause menu
            pauseOps = new List<string>();
            pauseOps.Add("Resume Game");
            pauseOps.Add("Instructions (currently exits)");
            pauseOps.Add("Exit Game");

            //game over menu
            gameOverOps = new List<string>();
            gameOverOps.Add("Start a New Game");
            gameOverOps.Add("Instructions (currently exits)");
            gameOverOps.Add("Exit Game");
 
            //start with the top option
            sel = 0;

            //Text For menu 
            titleFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            menuFont = Game.Content.Load<SpriteFont>("mFont");

            //picture for death screen
            deathScreen = Game.Content.Load<Texture2D>("textures/deathScreen");

            //picture for instructions
            instructionScreen = Game.Content.Load<Texture2D>("textures/instructions");

            //if the instruction screen is larger than the display size, we have to scale it
            //assumes we never run on a display that's taller than it is wide
            if (Game1.screenSize.Y < instructionScreen.Height)
            {
                float aR = instructionScreen.Width / instructionScreen.Height;
                int sideBuffer = Math.Abs((int)(Game1.screenSize.Y * aR) - instructionScreen.Width)/2;
                instRect = new Rectangle(sideBuffer, 0, (int)Game1.screenSize.X - sideBuffer*2, (int)Game1.screenSize.Y);
            }
            else
            {
                Vector2 center;
                center.Y = Game1.screenSize.Y / 2;
                center.X = Game1.screenSize.X / 2;
                instRect = new Rectangle((int)center.X - instructionScreen.Width / 2,
                    (int)center.Y - instructionScreen.Height / 2,
                    instructionScreen.Width,
                    instructionScreen.Height);
            }

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            menuSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            //get the keystates
            bool sDown = keyboardState.IsKeyDown(Keys.S);
            bool downDown = keyboardState.IsKeyDown(Keys.Down);
            bool wDown = keyboardState.IsKeyDown(Keys.W);
            bool upDown = keyboardState.IsKeyDown(Keys.Up);
            bool escDown = keyboardState.IsKeyDown(Keys.Escape);
            bool enter = keyboardState.IsKeyDown(Keys.Enter);

            //need to save whatever the previous menu was so we can go back to that
            //when we do the esc key
            if (previousGameState == Game1.GameState.MainMenu)
                savedGameState = Game1.GameState.MainMenu;
            if (previousGameState == Game1.GameState.PauseMenu)
                savedGameState = Game1.GameState.PauseMenu;
            if (previousGameState == Game1.GameState.GameOver)
                savedGameState = Game1.GameState.GameOver;

            switch(Game1.currentGameState)
            {
                case Game1.GameState.MainMenu:
                    #region Main Menu
                    if (!keyDown && (sDown || downDown))
                    {
                        keyDown = true;
                        moveDown = true;
                    }
                    if (!keyDown && (wDown || upDown))
                    {
                        keyDown = true;
                        moveUp = true;
                        moveDown = false; //ignore down
                    }
                    if (moveDown)
                    {
                        sel++;
                        if (sel > selMax)
                            sel = 0;
                        moveDown = false;
                    }
                    if (moveUp)
                    {
                        sel--;
                        if (sel < 0)
                            sel = selMax;
                        moveUp = false;
                    }

                    if (enter)
                    {
                        enter = false; //reset for next time
                        if (sel == 0) Game1.currentGameState = Game1.GameState.NewGame;
                        if (sel == 1) Game1.currentGameState = Game1.GameState.InstructionScreen;
                        if (sel == 2) Game1.currentGameState = Game1.GameState.GameExit;
                    }
                #endregion
                    break;
                case Game1.GameState.InstructionScreen:
                    #region Instruction Screen area
                    if (escDown)
                        Game1.currentGameState = savedGameState;
                    #endregion
                    break;
                case Game1.GameState.InGame:
                    #region In Game
                    if (!keyDown && escDown)
                    {
                        Game1.currentGameState = Game1.GameState.PauseMenu;
                    }
                    #endregion
                    break;
                case Game1.GameState.PauseMenu:
                    #region Pause Menu
                    if (!keyDown && (sDown || downDown))
                    {
                        keyDown = true;
                        moveDown = true;
                    }
                    if (!keyDown && (wDown || upDown))
                    {
                        keyDown = true;
                        moveUp = true;
                        moveDown = false; //ignore down
                    }
                    if (moveDown)
                    {
                        sel++;
                        if (sel > selMax)
                            sel = 0;
                        moveDown = false;
                    }
                    if (moveUp)
                    {
                        sel--;
                        if (sel < 0)
                            sel = selMax;
                        moveUp = false;
                    }

                    if (enter)
                    {
                        enter = false; //reset for next time
                        if (sel == 0) Game1.currentGameState = Game1.GameState.InGame;
                        if (sel == 1) Game1.currentGameState = Game1.GameState.InstructionScreen;
                        if (sel == 2) Game1.currentGameState = Game1.GameState.GameExit;
                    }
                    #endregion
                    break;
                case Game1.GameState.GameOver:
                    #region GameOver
                    if (!keyDown && (sDown || downDown))
                    {
                        keyDown = true;
                        moveDown = true;
                    }
                    if (!keyDown && (wDown || upDown))
                    {
                        keyDown = true;
                        moveUp = true;
                        moveDown = false; //ignore down
                    }
                    if (moveDown)
                    {
                        sel++;
                        if (sel > selMax)
                            sel = 0;
                        moveDown = false;
                    }
                    if (moveUp)
                    {
                        sel--;
                        if (sel < 0)
                            sel = selMax;
                        moveUp = false;
                    }

                    if (enter)
                    {
                        enter = false; //reset for next time
                        if (sel == 0) Game1.currentGameState = Game1.GameState.NewGame;
                        if (sel == 1) Game1.currentGameState = Game1.GameState.InstructionScreen;
                        if (sel == 2) Game1.currentGameState = Game1.GameState.GameExit;
                    }
                #endregion
                    break;

            }
            //save the current gamestate for use next time
            previousGameState = Game1.currentGameState;

            //reset for next time through
            keyDown = sDown || downDown || wDown || upDown || escDown;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int c = 0;
            switch (Game1.currentGameState)
            {
                case Game1.GameState.MainMenu:
                    #region Main Menu
                    Game.GraphicsDevice.Clear(Color.Black);
                    menuSpriteBatch.Begin();
                    menuSpriteBatch.DrawString(titleFont, title,
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) - 50), Color.Gold);
                    for (int i = 0; i <= 2; i++)
                    {
                        if (sel == i) mColor = Color.Red;
                        menuSpriteBatch.DrawString(menuFont, menuOps[i],
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) + c), mColor);
                        mColor = Color.WhiteSmoke;
                        c += 20;
                    }
                    menuSpriteBatch.End();
                    #endregion
                    break;
                case Game1.GameState.InstructionScreen:
                    #region Instruction Screen
                    Game.GraphicsDevice.Clear(Color.Black);
                    menuSpriteBatch.Begin();
                    menuSpriteBatch.Draw(instructionScreen, instRect, Color.White);
                    menuSpriteBatch.End();
                    #endregion
                    break;
                case Game1.GameState.NewGame:
                    #region New Game
                    Game.GraphicsDevice.Clear(Color.Black);
                #endregion
                    break;
                case Game1.GameState.PauseMenu:
                    #region Pause Menu
                    Game.GraphicsDevice.Clear(Color.Black);
                    menuSpriteBatch.Begin();
                    menuSpriteBatch.DrawString(titleFont, title,
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) - 50), Color.Gold);
                    for (int i = 0; i <= 2; i++)
                    {
                        if (sel == i) mColor = Color.Red;
                        menuSpriteBatch.DrawString(menuFont, pauseOps[i],
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) + c), mColor);
                        mColor = Color.WhiteSmoke;
                        c += 20;
                    }
                    menuSpriteBatch.End();
                    #endregion
                    break;
                case Game1.GameState.GameOver:
                    #region Game Over
                    Game.GraphicsDevice.Clear(Color.Black);
                    menuSpriteBatch.Begin();
                    menuSpriteBatch.Draw(deathScreen,new Rectangle(0,0,deathScreen.Width,deathScreen.Height),
                        Color.White);
                    menuSpriteBatch.DrawString(titleFont, title,
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) - 50), Color.Gold);
                    for (int i = 0; i <= 2; i++)
                    {
                        if (sel == i) mColor = Color.Red;
                        menuSpriteBatch.DrawString(menuFont, gameOverOps[i],
                                            new Vector2((Game.Window.ClientBounds.Width / 2) - (titleFont.MeasureString(title).X / 2),
                                                        (Game.Window.ClientBounds.Height / 2) - (titleFont.MeasureString(title).Y / 2) + c), mColor);
                        mColor = Color.WhiteSmoke;
                        c += 20;
                    }
                    menuSpriteBatch.End();
                #endregion
                    break;
                case Game1.GameState.GameExit:
                    #region Game Exit
                    Game.GraphicsDevice.Clear(Color.Black);
                    #endregion
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
