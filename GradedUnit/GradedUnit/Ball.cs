//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

//namespace GradedUnit
//{
//    class Ball
//    {
        
//        //class to hold the structure and properties of the ball
//        private Texture2D texture; // holds the image of the ball 
//        private Vector3 pos; //holds the position of the ball 
//        private Vector2 orig; // holds the origin of the image aids in drawing of the image 
//        private float size; //sets the size used to scale the image when being drawn 
//        private Rectangle recta; // holds dimensions of the graphic 
//        private float rotation;//holds the rotation for the ball 
//        private Vector3 velocity; //holds the speed and direction of the ball 
//        private BoundingSphere bsph; //holds the bounding sphere for the object 
//        private Boolean visible; // holds if the object should be visible or not 
//        private float power;//

//        // getter for texture 
//        public Texture2D getTexture()
//        {
//            return texture;
//        }
//        // getter for position
//        public Vector3 getPosition()
//        {
//            return pos;
//        }
//        // getter for origin
//        public Vector2 getOrigin()
//        {
//            return orig;
//        }
//        // getter for size
//        public float getSize()
//        {
//            return size;
//        }
//        // getter for rectangle 
//        public Rectangle getRectangle()
//        {
//            return recta;
//        }
//        // getter for rotation 
//        public float getRotation()
//        {
//            return rotation;
//        }
//        // getter for velocity 
//        public Vector3 getVelo()
//        {
//            return velocity;
//        }
//        // getter for bounding sphere 
//        public BoundingSphere getBoundingSphere()
//        {
//            return bsph;
//        }
//        // getter for visible 
//        public bool getVisible()
//        {
//            return visible;
//        }
//        // getter for power 
//        public float getPower()
//        {
//            return power;
//        }

//        //setter for texture 
//        public void setTexture(Texture2D texturein)
//        {
//            texture = texturein;
//        }
//        //setter for position
//        public void setPosition(Vector3 Posin)
//        {
//            pos = Posin;
//        }
//        //setter for origin
//        public void setOrig(Vector2 origin)
//        {
//            orig = origin;
//        }
//        //setter for size
//        public void setSize(float sizein)
//        {
//            size = sizein;
//        }
//        //setter for rectangle
//        public void setRectangle(Rectangle rectain)
//        {
//            recta = rectain;
//        }
//        //setter for rotation
//        public void setRotation(float rotationin)
//        {
//            rotation = rotationin;
//        }
//        //setter for velocity
//        public void setVelocity(Vector3 veloin)
//        {
//            velocity = veloin;
//        }
//        //setter for boundingsphere
//        public void setBoundingSphere(BoundingSphere bsphin)
//        {
//            bsph = bsphin;
//        }
//        //setter for visible 
//        public void setVisible(bool visin)
//        {
//            visible = visin;
//        }
//        //setter for power
//        public void setPower(float powin)
//        {
//            power = powin;
//        }

//        public Ball()
//        {
//            texture = Content.Load<Texture2D>("Ball");
//            pos.X = 0;
//            pos.Y = 0;
//            orig.X = texture.Width/2;
//            orig.Y = texture.Height/2;
//            size = 1f;

//            recta.Width = (int)(texture.Width*size);
//            recta.Height = (int)(texture.Height * size);
//            velocity.X = 0;
//            velocity.Y = 0;
           


//            //ball.image = Content.Load<Texture2D>("BALL"); /*load ball*/
//            //ball.size = .75f;  /* Size ratio of the ball */
//            //ball.origin.X = ball.image.Width / 2; /*set origin to center of image*/
//            //ball.origin.Y = ball.image.Height / 2; /* set origin to center of image */
//            //ball.rect.Width = (int)(ball.image.Width * ball.size);
//            //ball.rect.Height = (int)(ball.image.Height * ball.size);
//            //ball.rotationspeed = .1f;

//        }


//    }
//}
