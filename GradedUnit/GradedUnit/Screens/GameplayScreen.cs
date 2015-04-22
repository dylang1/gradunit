
#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GradedUnit
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Variables

        ContentManager content;

        SpriteFont gameFont;
        //creates a bat object for player bats 
        Bat P1bat;
        Bat P2bat; 
        Rectangle ScreenBoundary;
        Ball ball;
        int bricksWidth = 10;
        int bricksHeight = 5;
        Texture2D brickImage;
        Bricks[,] bricks;
        int lives = 3; //sets initial lives to 3 
        Random random = new Random();


        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }



        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            ScreenBoundary = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("quartz4");// loads the font which will be used throughout the game 
            Texture2D battexture = content.Load<Texture2D>("bat");// loads the texture of the batt 
            Texture2D balltexture = content.Load<Texture2D>("ball"); // loads the texture of the ball 
             brickImage = content.Load<Texture2D>("brick");//loads the texture of the brick 

            P1bat = new Bat(battexture, ScreenBoundary);//creats a new bat object called P1bat
            ball = new Ball(balltexture, ScreenBoundary);//creates a new ball object 
            StartGame();
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);
            
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        private void StartGame()
        {
            P1bat.startPosP1(); //sets the starting position of the ball 
            ball.StartPosBall(P1bat.GetBoundary());//sets starting pos of ball 

            bricks = new Bricks[bricksWidth, bricksHeight];

            for (int i = 0; i < bricksHeight; i++)
            {
                Color colour = Color.White;

                switch(i)
                {
                    case 0:
                        colour = Color.HotPink;
                        break;
                    case 1:
                        colour = Color.Purple;
                        break;
                    case 2:
                        colour = Color.MidnightBlue;
                        break;
                    case 3:
                        colour = Color.Goldenrod;
                        break;
                    case 4:
                        colour = Color.Green;
                        break;
                }
                for (int x =0; x<bricksWidth; x++)
                {
                    bricks[x, i] = new Bricks(brickImage, new Rectangle(x * brickImage.Width, i * brickImage.Height, brickImage.Width, brickImage.Height), colour);
                }
            }
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            
            if (IsActive)
            {
                ball.UpdatePos();
                foreach(Bricks brick in bricks)
                {
                    brick.CollisionCheck(ball);
                }
                ball.BatCollision(P1bat.GetBoundary());
                if (ball.BottomCheck())
                {
                    lives -= 1;
                }

                if (ball.TopCheck())
                {
                    lives -= 1;
                }

            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// 
        /// TODO fix movement logic 
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];



            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if(input.IsP1Right(/*ControllingPlayer*/) == true)
                {
                    P1bat.MoveBatRight();
                }
                if(input.IsP1Left(/*ControllingPlayer*/) == true)
                {
                    P1bat.MoveBatLeft();
                }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            foreach (Bricks brick in bricks)
                brick.Draw(spriteBatch);
            spriteBatch.DrawString(gameFont,lives.ToString(),new Vector2(0,0),Color.Blue);
            P1bat.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
