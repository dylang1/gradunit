using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GradedUnit
{
  class TextBox
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle textBox;
        Texture2D debugColor;
        SpriteFont font;
        String text;
        String parsedText;
        String typedText;
        double typedTextLength;
        int delayInMilliseconds;
        bool isDoneDrawing;
        public TextBox(Texture2D debugColor,SpriteFont font,string text,Rectangle textBox)
        {
            this.debugColor = debugColor;
            this.font = font;
            this.text = text;
            this.textBox = textBox;
            parsedText = parseText(text);
            delayInMilliseconds = 50;
            isDoneDrawing = false; 
        }
        String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');
            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > textBox.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;

        }
    }
}
