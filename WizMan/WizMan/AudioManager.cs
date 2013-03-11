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
    public class AudioManager : Microsoft.Xna.Framework.GameComponent
    {
        public AudioManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 
        //declare and then load any sound effects we may need to play
        SoundEffect jumpSound;
        SoundEffect fireSound;
        SoundEffect windSound;
        SoundEffect iceSound;
        SoundEffect shockSound;
        SoundEffect footSteps;

        int elapsedFire = 0;
        int elapsedIce = 0;
        int elapsedWind = 0;
        int elpsedShock = 0;
        int elapsedFootstep = 0;

        public override void Initialize()
        {
            jumpSound = Game.Content.Load<SoundEffect>("sounds/jump");
            fireSound = Game.Content.Load<SoundEffect>("sounds/fire");
            iceSound = Game.Content.Load<SoundEffect>("sounds/ice");
            windSound = Game.Content.Load<SoundEffect>("sounds/jump");
            shockSound = Game.Content.Load<SoundEffect>("sounds/jump");
            footSteps = Game.Content.Load<SoundEffect>("sounds/footsteps");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void playJumpSound()
        {
            jumpSound.Play(0.25f, 0, 0);
        }

        public void playFireSound()
        {
            fireSound.Play(.25f, 0.5f, 0);
        }

        public void playIceSound()
        {
            iceSound.Play(.25f, .5f, 0);
        }

        public void playWindSound()
        {
            windSound.Play(1, 0, 0);
        }

        public void playShockSound()
        {
            shockSound.Play(1, 0, 0);
        }

        public void playFootSteps(GameTime gameTime)
        {
            elapsedFootstep += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedFootstep > footSteps.Duration.Milliseconds)
            {
                elapsedFootstep = 0;
                footSteps.Play(.1f, 0, 0);
            }
            

        }
    }
}
