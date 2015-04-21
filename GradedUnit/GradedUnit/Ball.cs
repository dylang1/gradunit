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
        Vector2 motion;
        Vector2 pos;
        float speed = 3f;
        Texture2D texture;
        Rectangle boundary;
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
        public Vector2 getOrigin()
        {
            return orig;
        }
        // getter for size
        public float getSize()
        {
            return size;
        }
        // getter for rectangle 
        public Rectangle getRectangle()
        {
            return recta;
        }
        // getter for rotation 
        public float getRotation()
        {
            return rotation;
        }
        // getter for velocity 
        public Vector3 getVelo()
        {
            return velocity;
        }
        // getter for bounding sphere 
        public BoundingSphere getBoundingSphere()
        {
            return bsph;
        }
        // getter for visible 
        public bool getVisible()
        {
            return visible;
        }
        // getter for power 
        public float getPower()
        {
            return power;
        }

        //setter for texture 
        public void setTexture(Texture2D texturein)
        {
            texture = texturein;
        }
        //setter for position
        public void setPosition(Vector2 Posin)
        {
            pos = Posin;
        }
        //setter for origin
        public void setOrig(Vector2 origin)
        {
            orig = origin;
        }
        //setter for size
        public void setSize(float sizein)
        {
            size = sizein;
        }
        //setter for rectangle
        public void setRectangle(Rectangle rectain)
        {
            recta = rectain;
        }
        //setter for rotation
        public void setRotation(float rotationin)
        {
            rotation = rotationin;
        }
        //setter for velocity
        public void setVelocity(Vector3 veloin)
        {
            velocity = veloin;
        }
        //setter for boundingsphere
        public void setBoundingSphere(BoundingSphere bsphin)
        {
            bsph = bsphin;
        }
        //setter for visible 
        public void setVisible(bool visin)
        {
            visible = visin;
        }
        //setter for power
        public void setPower(float powin)
        {
            power = powin;
        }
#endregion

        public Ball(Texture2D texture , Rectangle boundary)
        {
            this.texture = texture;
            this.boundary = boundary;
        }
         public void UpdatePos()
        {
            pos += motion *= speed;
            CollisionCheck();
        }
        private void CollisionCheck()
         {
             if (pos.X < 0)
             {
                 pos.X = 0;
                 motion.X *= -1;
             }
             if (pos.X + texture.Width > boundary.Width)
             {
                 pos.X = boundary.Width - texture.Width;
                 motion.X *= -1;
             }

         }
        // checks if the ball has left the bottom of the screen 
        public bool BottomCheck()
        {
            if (pos.Y > boundary.Height)
                return true;
            return false;
        }

        public bool TopCheck()
        {
            if (pos.Y == 0)
                return true;
            return false;
        }

        //checks if the ball hist the bat 
        public void PaddleCollision(Rectangle batloc)
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

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.White);
        }
    }
}
