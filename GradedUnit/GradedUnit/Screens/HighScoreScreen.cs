using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GradedUnit
{
    class HighScoreScreen : MenuScreen 
    {



        string name = "";
        string score= "";
        string mode = "" ;
        int inc = 1;
        


        public HighScoreScreen() : base("HighScores")
        {
            loadDb();
            //Draw();
        }
        public void loadDb()
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:/Documents/GitHub/gradunit/GradedUnit/HighScores.mdb";
            string strSelect = "SELECT HighScores.Score, HighScores.ModeType, Player.Name FROM (HighScores INNER JOIN Player ON HighScores.[user id] = Player.ID AND HighScores.[user id] = Player.ID)ORDER BY HighScores.Score DESC, Player.Name";

        

            DataSet ds = new DataSet();
            OleDbConnection con = null;
            try
            {
                con = new OleDbConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBoxScreen message = new MessageBoxScreen("Error: Failed to create a database connection. \n{0}" + ex.Message, true);
                return;
            }
            try
            {
                OleDbCommand accessCmd = new OleDbCommand(strSelect, con);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(accessCmd);
                con.Open();
                dataAdapter.Fill(ds,"HighScores");
            }
            catch (Exception ex)
            { MessageBoxScreen message = new MessageBoxScreen("Error: Failed to retrieve the data . \n{0}" + ex.Message, true);}
            finally{ con.Close();}

            //todo add in for loop to increase [inc] to loop through each row upto say a max of x ammount 
            DataRow dRow = ds.Tables["HighScores"].Rows[inc];
            score = dRow.ItemArray.GetValue(0).ToString();
            mode = dRow.ItemArray.GetValue(1).ToString();
            name = dRow.ItemArray.GetValue(2).ToString();
           

        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            sBatch.Begin();
            sBatch.DrawString(font, score, new Vector2(250, 100), Color.White);
            sBatch.DrawString(font, name, new Vector2(0, 100), Color.White);
            sBatch.DrawString(font, mode, new Vector2(500, 100), Color.White);
            sBatch.End();
        }


        //HighScoreScreen();
    }
}
