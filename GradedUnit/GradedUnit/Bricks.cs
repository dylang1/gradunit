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

        Texture2D texture;
        Rectangle position;
        Color colour;
        bool visible;

        public Rectangle Position
        {
            get { return position; }
        }
        public Bricks(Texture2D texture, Rectangle position,Color colour)
        {
            this.texture = texture;
            this.position = position;
            this.colour = colour;
            this.visible = true; 
        }

        public void CollisionCheck(Ball ball)
        {
            if(visible && ball.Boundary.Intersects(position))
            visible = false;
            ball.Deflection(this);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(texture, position, colour);
        }



    }
}
