using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GradedUnit
{
    class Ball
    {
        Vector2 motion; //holds the motion for tha ball 
        Vector2 pos; // holds the position of the ball 
        float speed = 1f; // holds the speed of the ball 
        Texture2D texture; // holds the texture of the ball 
        Rectangle scrboundary;//holds the screenboundayy used to determine collisions and stuff 
        Rectangle boundary;//holds the boundary of the ball 
        const float initialSpeed = 5f;// sets the initial speed of the ball 
        Random random = new Random();// new random number generetaor 
        bool collision; // holds if the ball has collided or not 
        #region Methods 


        //constructor to create a new instance of ball 
        public Ball(Texture2D texture, Rectangle scrboundary)
        {
            boundary = new Rectangle(0, 0, texture.Width, texture.Height);
            this.texture = texture;
            this.scrboundary = scrboundary;
        }

        //method to create a boundary round the ball 
        public Rectangle Boundary
        {
            get
            {
                boundary.X = (int)pos.X;
                boundary.Y = (int)pos.Y;
                return boundary;
            }
        }

        //method to update the position of the ball 
        public void UpdatePos()
        {
            collision = false;
            pos += (motion * speed);
            CollisionCheck();
            speed += 0.01f;
        }
        //checks if the ball has collided with the walls 
        private void CollisionCheck()
        {
            if (pos.X < 0)
            {
                pos.X = 0;
                motion.X *= -1;
            }
            if (pos.X + texture.Width > scrboundary.Width)
            {
                pos.X = scrboundary.Width - texture.Width;
                motion.X *= -1;
            }
            if (pos.Y < 0)
            {
                pos.Y = 0;
                motion.Y *= -1;
            }
        }
        // checks if the ball has left the bottom of the screen 
        public bool BottomCheck()
        {
            if (pos.Y > scrboundary.Height)
                return true;
            return false;
        }
        //checks if the ball has hit the top of the screen 
        public bool TopCheck()
        {
            if (pos.Y <= 0)
                return true;
            return false;
        }

        //checks if the ball hist the bat 
        public bool BatCollision(Rectangle batloc,bool isP1,bool lastcollp1)
        {
            Rectangle ballloc = new Rectangle((int)pos.X,(int)pos.Y,texture.Width,texture.Height);
            if (batloc.Intersects(ballloc) && isP1)
            {
                pos.Y = batloc.Y - texture.Height;
                motion.Y *= -1;
                lastcollp1 = true;
                return lastcollp1;
            }
            if (batloc.Intersects(ballloc) && !isP1)
            {
                //pos.Y = batloc.Y - texture.Height;
                motion.Y *= -1;
                lastcollp1 = false;
                    return lastcollp1;
            }
            return lastcollp1;
            
        }

        //method to check collision 
        public void BatCollision(Rectangle batloc, bool isP1)
        {
            Rectangle ballloc = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            if (batloc.Intersects(ballloc) && isP1)
            {
                pos.Y = batloc.Y - texture.Height;
                motion.Y *= -1;

            }
            if (batloc.Intersects(ballloc) && !isP1)
            {
                //pos.Y = batloc.Y - texture.Height;
                motion.Y *= -1;
            }

        }
        // method to return the motion of the ball 
        public Vector2 getMotion()
        {
            return this.motion;
        }

        //method to deflect the ball 
        public void Deflection(Bricks brick)
        {

            if (!collision )
            {
                motion.Y *= -1;
                collision = true;
            }
            
        }
        //method to set the starting position of the ball 
        public void StartPosBall(Rectangle batPosition,bool isp1)
        {
            motion = new Vector2(0, 0);
            speed = initialSpeed;
            if (isp1)
                pos.Y = batPosition.Y - texture.Height;
            else
                pos.Y = batPosition.Y + texture.Height;
            pos.X = batPosition.X + (batPosition.Width - texture.Width) / 2;
        }
        //method to start the motion of the ball 
        public void StartMotion()
        {
            motion = new Vector2(random.Next(1, 5), -random.Next(1, 5));
            motion.Normalize();
        }

        //method to draw the ball 
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.Crimson);
        }
        #endregion
    }
}
