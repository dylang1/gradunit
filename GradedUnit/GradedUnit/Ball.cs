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
        Rectangle boundary;
        const float initialSpeed = 1.01f;
        bool collision;
        #region Getters Setters
        // getter for texture 
        public Texture2D getTexture()
        {
            return texture;
        }
        // getter for position
        public Vector2 getPosition()
        {
            return pos;
        }
        // getter for origin
        #endregion
        //constructor to create a new instance of ball 
        public Ball(Texture2D texture, Rectangle scrboundary)
        {
            boundary = new Rectangle(0, 0, texture.Width, texture.Height);
            this.texture = texture;
            this.scrboundary = scrboundary;
        }

        public Rectangle Boundary
        {
            get
            {
                boundary.X = (int)pos.X;
                boundary.Y = (int)pos.Y;
                return boundary;
            }
        }
        public void UpdatePos()
        {
            collision = false;
            pos += motion *= speed;
            CollisionCheck();
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
            if (pos.Y < 0)
                return true;
            return false;
        }

        //checks if the ball hist the bat 
        public void BatCollision(Rectangle batloc)
        {
            Rectangle ballloc = new Rectangle(
            (int)pos.X,
            (int)pos.Y,
            texture.Width,
            texture.Height);
            if (batloc.Intersects(ballloc))
            {
                pos.Y = batloc.Y - texture.Height;
                motion.Y *= -1;
            }
        }

        public void Deflection(Bricks brick)
        {

            if (!collision)
            {
                motion.Y *= -1;
                collision = true;
            }
            
        }
        public void StartPosBall(Rectangle batPosition)
        {
            motion = new Vector2(1,-1);
            speed = initialSpeed;
            pos.Y = batPosition.Y - texture.Height;
            pos.X = batPosition.X + (batPosition.Width - texture.Width) / 2;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.Red);
        }
    }
}
