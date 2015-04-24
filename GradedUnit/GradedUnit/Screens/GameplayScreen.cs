
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
        // manager that will use the content pipeline to load evertyhing 
        ContentManager content;
        // the font that the game will use 
        SpriteFont gameFont;
        //creates a bat object for player bats 
        Bat P1bat;
        Bat P2bat; 
        //sets the screen boundary so that we can check if the ball or batt hits the sied of the screen
        Rectangle ScreenBoundary;
        //creates a new Ball 
        Ball ball;
        // how many bricks wide will be displayed 
        int bricksWidth = 15;
        // how many bricks high will be displayed 
        int bricksHeight = 5;
        TextBox textBox;
        // texture for the bricks 
        Texture2D brickImage;
        // creates a new array of bricks 
        Bricks[,] bricks;
        //creates a new databaseconnection 
        DbConn dbConn;
        //holds the score for the game
        int score = 0;
        //holds the userinput got enterring highscores 
        string userinput;
        
        //holds the gamemode for the database to right to 
        string gamemode = "CoOp";
        int lives = 4; //sets initial lives to 4
        // new random number generator 
        Random random = new Random();
        //checks if the writing to database has occured 
        bool isWritten = false; 


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
            dbConn = new DbConn();// loads teh database 
            dbConn.loadDb(gamemode);//loads the database 

            gameFont = content.Load<SpriteFont>("quartz4");// loads the font which will be used throughout the game 
            Texture2D battexture = content.Load<Texture2D>("bat");// loads the texture of the batt 
            Texture2D balltexture = content.Load<Texture2D>("ball"); // loads the texture of the ball 
             brickImage = content.Load<Texture2D>("brick");//loads the texture of the brick 
            P1bat = new Bat(battexture, ScreenBoundary);//creats a new bat object called P1bat
            ball = new Ball(balltexture, ScreenBoundary);//creates a new ball object 
            P2bat = new Bat(battexture, ScreenBoundary);//creates a new object called p2bat
            StartGame();//starts the game 
            
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        private void StartGame()
        {
            isWritten = false; // sets if the written method has been done to false 
            P1bat.startPosP1(); //sets the starting position of the bat
            P2bat.startPosP2();// sets the starting position of the bat 
            ball.StartPosBall(P1bat.GetBoundary(),true);//sets starting pos of ball 

            bricks = new Bricks[bricksWidth, bricksHeight];//creates a new array of width and high changes colour depending on which row the bricks are in 

            for (int i = 0; i < bricksHeight; i++)
            {
                Color colour = Color.White;

                switch(i)
                {
                    case 0:
                        colour = Color.HotPink;
                        break;
                    case 1:
                        colour = Color.Red;
                        break;
                    case 2:
                        colour = Color.MidnightBlue;
                        break;
                    case 3:
                        colour = Color.Orange;
                        break;
                    case 4:
                        colour = Color.Green;
                        break;
                }
                for (int x =0; x<bricksWidth; x++)
                {
                    bricks[x, i] = new Bricks(brickImage, new Rectangle(x * brickImage.Width, i * brickImage.Height +300, brickImage.Width, brickImage.Height), colour); // creates rectangle ofr each brick created 
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
          
            // if the screen is active 
            
            if (IsActive)
            {
                if (lives != 0)
                {
                    //moves tbe ball position 
                    ball.UpdatePos();
                    // checks the collision for each the bricks 
                    foreach (Bricks brick in bricks)
                    {
                        score += brick.CollisionCheck(ball);
                    }
                    //checsk if the ball hits the bat 
                    ball.BatCollision(P1bat.GetBoundary(),true);
                    ball.BatCollision(P2bat.GetBoundary(),false);
                    //checsk fi the ball leaves the bottom of the screen if so remove lives by one and set it back to ontop of the bat 
                    if (ball.BottomCheck())
                    {
                        lives -= 1;
                        ball.StartPosBall(P1bat.GetBoundary(),true);

                    }
                    //checks if the ball leave the top of the screen if so remove lives by one and set it back to ontop of the bat 
                    if (ball.TopCheck())
                    {
                        lives -= 1;
                        ball.StartPosBall(P1bat.GetBoundary(),true);
                    }
                }
                //checks if the lives are = 0 
                if (lives == 0)
                {
                    //checks if this method has been run before 
                    if (!isWritten)
                    {
                        //sets the inputscore to the score of the game 
                        int inputscore = (int)score;
                        //moves the ball back ontop of the bat 
                        ball.StartPosBall(P1bat.GetBoundary(),true);
                        //asks the user for there name 
                        userinput = Microsoft.VisualBasic.Interaction.InputBox("Name", "Please Enter Your Name", "AAAAA").ToString();
                        // max length of 10 characters 
                        int maxstrlength = 10;
                        // if the user inputs more than 10 chars it will cut off any chars moe than 10 
                        if (userinput.Length > maxstrlength)
                        { userinput = userinput.Substring(0, maxstrlength); }
                        //addsthe name to the player database 
                        dbConn.addNametoDb(userinput);
                        //adds the gamemode and score to the highscores table 
                        dbConn.addtoDB(gamemode,inputscore);
                        //sets that this method has been run before 
                        isWritten = true; 

                    }
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


            //aslong as the game isnt paused 
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // if the key for p1 to move right is pressed move player one right
                if (input.IsP1Right(/*ControllingPlayer*/) == true)
                {
                    P1bat.MoveBatRight();
                }
                // if the key for player 1 to move left is pressed move player one left 
                if (input.IsP1Left(/*ControllingPlayer*/) == true)
                {
                    P1bat.MoveBatLeft();
                }
                // if the key for player 1 to move left is pressed move player two left 
                if (input.IsP2left())
                    P2bat.MoveBatLeft();
                // if the key for player 2 to move left is pressed move player two right 
                if (input.IsP2Right())
                    P2bat.MoveBatRight();
                // if the key for player 1 to start the ge game and the ball motion is = 0 then start he motion of the ball  
                if (input.IsP1Start())
                {
                    if(ball.getMotion() == new Vector2(0,0) )
                    ball.StartMotion();
                }
                // if the key for player 2  to start the ge game and the ball motion is = 0 then start he motion of the ball 
                if (input.IsP2Start())
                {
                    if (ball.getMotion() == new Vector2(0, 0))
                        ball.StartMotion();
                }
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //draws a white background 
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            //draw each brick 
            foreach (Bricks brick in bricks)
                brick.Draw(spriteBatch);
            //draw the lives and score at the top of the screen 
            spriteBatch.DrawString(gameFont,"Lives: " + lives.ToString(),new Vector2(10,0),Color.Blue);
            spriteBatch.DrawString(gameFont,"Score: " + score.ToString(),new Vector2(170,0),Color.Blue);
            // if the lives are =0 then draw gamemover and tell teh user their score 
            if (lives == 0)
            {
                spriteBatch.DrawString(gameFont, "GAME OVER", new Vector2(255, 300), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(gameFont, "Score: " + score, new Vector2(255, 400), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);


            }
            //draw the bats and ball 
            P1bat.Draw(spriteBatch,Color.Red);
            P2bat.Draw(spriteBatch,Color.Blue);
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