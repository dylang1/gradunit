using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GradedUnit
{
    class HighScoreScreen : MenuScreen 
    {

        DbConn dbConn;
        MenuEntry dbSelectMenuEntry;
        int rowsrodraw = 0;
        int maxrows = 20;

        string[] dboptions = { "CoOp", "Comp " };
        

        enum Options
        {
            Coop,
            Comp,
        }
        static Options currentOption = Options.Coop;
        public HighScoreScreen() : base("HighScores")
        {
            string coopString = "CoOp";
            dbSelectMenuEntry = new MenuEntry(string.Empty);
            dbConn = new DbConn();
            SetMenuEntryText();
            dbSelectMenuEntry.Selected += DBSelect;
            MenuEntries.Add(dbSelectMenuEntry);
            // cooop load 
            dbConn.loadDb(coopString);
            ////CompLoad
            //loadDb(compString);
            //Draw();
        }

        void SetMenuEntryText()
        {
            dbSelectMenuEntry.Text = "Mode:" + currentOption;
        } 
        void DBSelect(object sender, PlayerIndexEventArgs e)
        {
            currentOption++;
            if (currentOption > Options.Comp)
            {
                currentOption = 0; 
            }
            if (currentOption == Options.Coop)
            {

                rowsrodraw = 0;
                string CoOp = "CoOp";
               dbConn.loadDb(CoOp);
               rowsrodraw = dbConn.checkRows();
            }
            else
            {
                rowsrodraw = 0; 
                string comp = "Comp";
                dbConn.loadDb(comp);
                rowsrodraw = dbConn.checkRows();
            }

            SetMenuEntryText();
        }
        

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            sBatch.Begin();
            sBatch.DrawString(font, "Mode; " + currentOption, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 10), Color.White);
            int i = 1;
            int count = 0;
            rowsrodraw = dbConn.checkRows();
            while( i <= rowsrodraw && i <= maxrows )
            {
                dbConn.Draw(font,sBatch,count);
                i++;
             
           }
            sBatch.End();
        }

        
     
        //HighScoreScreen();
    }
}
