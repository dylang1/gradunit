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
        int scorevalue;

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
            this.scorevalue = 10;
        }

        public int CollisionCheck(Ball ball)
        {
            this.scorevalue = 0;
            if (visible && ball.Boundary.Intersects(position))
            {
                visible = false;
                ball.Deflection(this);
                return this.scorevalue;
            }
            return scorevalue;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(texture, position, colour);
        }



    }
}
