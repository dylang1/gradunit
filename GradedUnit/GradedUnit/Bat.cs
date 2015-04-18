using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GradedUnit
{

    // class to create and hold functions ofr each bat 
    class Bat
    {
        Vector2 pos;//holds the position of the bat 
        Vector2 motion; //holds the motion of the bat 
        float speed = 10f;//holds the speed at which the bat will move at
        Texture2D texture; // holds the texture for the bat 

        Rectangle boundary; // holds the boundary for the screen 


        //constructor for creating a new bat 
        public Bat(Texture2D texture, Rectangle boundary)
        {
            this.texture = texture;
            this.boundary = boundary;
        }

        // function to move the bat to the left 
        public void MoveBatLeft()
        {
            motion = Vector2.Zero;
            motion.X = -1;
            motion.X *= speed; // multiplyts the motion by speed to see how fast the paddle moves 
            pos += motion;
            BoundaryCheck();
        }


        //function to move the bat right 
        public void MoveBatRight()
        {
            motion = Vector2.Zero;
            motion.X = 1;
            motion.X *= speed;// multiplys the motion by speed to see how fast the paddle moves 
            pos += motion;
            BoundaryCheck();

        }
        //function to check if the bat has moved outside of the screeen if it has fix its position 
        public void BoundaryCheck()
        {
            if(pos.X<0) // cbecks so the the bat cant go off the left hand side of the screen 
            {
                pos.X = 0; 
            }
            if(pos.X + texture.Width > boundary.Width) // checks so the bat cant go off the right hand side of the screen 
            {
                pos.X = boundary.Width - texture.Width; // sets the position of the bat to 
            }
        }

        // draws a rectangle ontop of the bat which will be used to check if it collides with the ball or not 
        public Rectangle GetBoundary()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
        }

        public void startPosP1()
        {
            pos.X = (boundary.Width - texture.Width) / 2;// sets hte position to the midle of the screen 
            pos.Y = (boundary.Height - texture.Height -10);
        }
        //function to draw the bat 
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.Red);
        }
    }
}
