using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GradedUnit
{
    class Bricks
    {
        //variabls 
        Texture2D texture;
        Rectangle position;
        Color colour;
        bool visible;
        int scorevalue;

        //returns the position of the bricks 
        public Rectangle Position
        {
            get { return position; }
        }
        //constructor for the brikcs 
        public Bricks(Texture2D texture, Rectangle position,Color colour)
        {
            this.texture = texture;
            this.position = position;
            this.colour = colour;
            this.visible = true;
            this.scorevalue = 10;
        }
        //checks if the brick has collided with the ball if it has make the brick disappear and deflect the ball and return the scorevalue 
        public int CollisionCheck(Ball ball)
        {
            this.scorevalue = 0;
            if (visible && ball.Boundary.Intersects(position))
            {
                scorevalue = 10;
                visible = false;
                ball.Deflection(this);
                return this.scorevalue;
            }
            return scorevalue;
        }
        //if the bricks are visible draw them on the screen 
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(texture, position, colour);
        }



    }
}
