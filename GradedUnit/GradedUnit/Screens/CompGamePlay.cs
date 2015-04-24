
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
    class CompGamePlay : GameScreen
    {
        #region Variables

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
        // texture for the bricks 
        Texture2D brickImage;
        // creates a new array of bricks 
        Bricks[,] bricks;

        //inputscore to insert into db 
        int inputscore = 0;
        // holds the datbase constructo 
        DbConn dbConn;
        //player one score
        int p1score = 0;
        // player 2 score
        int p2score = 0;
        //string for the user input which will be added to the db 
        string userinput;
        // gamemode string 
        string gamemode = "Comp";
        int p1lives = 4; //sets initial lives to 4
        int p2lives = 4;//sets intial lives to 5 
        //holds if the last position is with player one 
        bool lastcollp1 = true;
        // new random number generator 
        Random random = new Random();
        //holds if the db has been written 
        bool isWritten = false;


        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CompGamePlay()
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
            dbConn = new DbConn();//CREATES THE DATABASSE CONNECTION OBJECT 
            dbConn.loadDb(gamemode);//LOADS TEH DATABASAE FOR THIS MODE 

            gameFont = content.Load<SpriteFont>("quartz4");// loads the font which will be used throughout the game 
            Texture2D battexture = content.Load<Texture2D>("bat");// loads the texture of the batt 
            Texture2D balltexture = content.Load<Texture2D>("ball"); // loads the texture of the ball 
            brickImage = content.Load<Texture2D>("brick");//loads the texture of the brick 
            P1bat = new Bat(battexture, ScreenBoundary);//creats a new bat object called P1bat
            ball = new Ball(balltexture, ScreenBoundary);//creates a new ball object 
            P2bat = new Bat(battexture, ScreenBoundary);//creates a new object called p2bat
            StartGame();//STARTS THE GAME 

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }
        //FUNCTGION TO START THE GAME 
        private void StartGame()
        {
            isWritten = false;
            P1bat.startPosP1(); //sets the starting position of the bat
            P2bat.startPosP2();
            ball.StartPosBall(P1bat.GetBoundary(),true);//sets starting pos of ball 
            //CREATES THE BRICKS AND SETS THE COLOUR DEPENDING ON WHICH ROW THEY ARE 
            bricks = new Bricks[bricksWidth, bricksHeight];

            for (int i = 0; i < bricksHeight; i++)
            {
                Color colour = Color.White;

                switch (i)
                {
                    case 0:
                        colour = Color.Red;
                        break;
                    case 1:
                        colour = Color.MidnightBlue;
                        break;
                    case 2:
                        colour = Color.Green;
                        break;
                    case 3:
                        colour = Color.Orange;
                        break;
                    case 4:
                        colour = Color.Pink;
                        break;
                }
                for (int x = 0; x < bricksWidth; x++)
                {
                    bricks[x, i] = new Bricks(brickImage, new Rectangle(x * brickImage.Width, i * brickImage.Height + 300, brickImage.Width, brickImage.Height), colour);
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
                if (p1lives != 0)
                {
                    ball.UpdatePos();
                    // checks the collision for each the bricks AND WHICH PLAYER TO AWWARD SCORE TO 
                    foreach (Bricks brick in bricks)
                    {
                        if (lastcollp1)
                            p1score += brick.CollisionCheck(ball);
                        else
                            p2score += brick.CollisionCheck(ball);
                    }
                    //checsk if the ball hits which bat 
                   lastcollp1= ball.BatCollision(P1bat.GetBoundary(), true,lastcollp1);
                    lastcollp1 = ball.BatCollision(P2bat.GetBoundary(), false,lastcollp1);
                    //checsk fi the ball leaves the bottom of the screen if so remove player one lives lives by one awward player 2 score
                    if (ball.BottomCheck())
                    {
                        p1lives -= 1;
                        ball.StartPosBall(P2bat.GetBoundary(),false);
                        lastcollp1 = false;
                        p2score += 20;

                    }
                    //checks if the ball leave the top of the screen if so remove player 2 lives by one and awward player 1 score 
                    if (ball.TopCheck())
                    {
                        p2lives -= 1;
                        ball.StartPosBall(P1bat.GetBoundary(),true);
                        lastcollp1 = true;
                        p1score += 20;
                    }
                }
                if (p1lives == 0 || p2lives ==0)//checks if gameover 
                {
                    //if player 1 > is bigger the player 2 score then p1 wins nad write that to database 
                    if (p1score> p2score)
                    {
                        inputscore = (int)p1score;
                    }
                    else {  inputscore = (int)p2score; }
                    if (!isWritten)//chesk if this method has been run before 
                    {
                        //resets pos of ball 
                        ball.StartPosBall(P1bat.GetBoundary(),true);
                        //gets the name of the user 
                        userinput = Microsoft.VisualBasic.Interaction.InputBox("Name", "Please Enter Your Name", "AAAAA").ToString();
                        // max length of 10 characters 
                        int maxstrlength = 10;
                        // if the user inputs more than 10 chars it will cut off any chars moe than 10 
                        if (userinput.Length > maxstrlength)
                        { userinput = userinput.Substring(0, maxstrlength); }
                        //add tehs name ot the player table 
                        dbConn.addNametoDb(userinput);
                        //ads the score and mode to the highscores table 
                        dbConn.addtoDB(gamemode, inputscore);

                        isWritten = true;

                    }
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// 
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
                    if (ball.getMotion() == new Vector2(0, 0))
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
            // draw the background as blakc 
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

           
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            //draws each brick 
            foreach (Bricks brick in bricks)
                brick.Draw(spriteBatch);
            //draw the players lives and scores at the top of the screen 
            spriteBatch.DrawString(gameFont, "Player 1 Lives: " + p1lives.ToString(), new Vector2(10, 0), Color.Blue);
            spriteBatch.DrawString(gameFont, "Player 1 Score: " + p1score.ToString(), new Vector2(350, 0), Color.Blue);
            spriteBatch.DrawString(gameFont, "Player 2 Lives: " + p2lives.ToString(), new Vector2(700, 0), Color.Pink);
            spriteBatch.DrawString(gameFont, "Player 2 Score: " + p2score.ToString(), new Vector2(1050, 0), Color.Pink);
            //checks which player has one the game if its p2 display p2 score and that p2 has one 
            if (p1lives == 0 || p2lives == 0 && p2score > p1score)
            {
                spriteBatch.DrawString(gameFont, "GAME OVER P2 Wins", new Vector2(255, 300), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(gameFont, "Score: " + p2score, new Vector2(255, 400), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);
            }
            if(p1lives == 0||p2lives == 0 && p1score>p2score )
            {
                spriteBatch.DrawString(gameFont, "GAME OVER P1 Wins", new Vector2(255, 300), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(gameFont, "Score: " + p1score, new Vector2(255, 400), Color.HotPink, MathHelper.ToRadians(0), new Vector2(0, 0), 3.5f, SpriteEffects.None, 0);
            }
            //draw the bats and ball 
            P1bat.Draw(spriteBatch, Color.Red);
            P2bat.Draw(spriteBatch, Color.Blue);
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