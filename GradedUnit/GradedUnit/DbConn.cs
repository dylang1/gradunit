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
using System.Diagnostics;
namespace GradedUnit
{
    class DbConn
    {

        DataRowCollection dra;
        int rowsrodraw = 0; 
        DataSet ds = new DataSet();
        OleDbConnection con = null;
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:/Documents/GitHub/gradunit/GradedUnit/HighScores.mdb";

        public void loadDb(string mode)
        {
            ds.Clear();
            string strSelect;
            if (mode == "CoOp")
            {
                strSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user id] = Player.ID AND HighScores.[user id] = Player.ID) WHERE (HighScores.ModeType = 'CoOp') ORDER BY HighScores.Score DESC, Player.Name;";
            }
            else { strSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user id] = Player.ID AND HighScores.[user id] = Player.ID) WHERE (HighScores.ModeType = 'Comp') ORDER BY HighScores.Score DESC, Player.Name;"; }
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
                dataAdapter.Fill(ds, "HighScores");
            }
            catch (Exception ex)
            { MessageBoxScreen message = new MessageBoxScreen("Error: Failed to retrieve the data . \n{0}" + ex.Message, true); }
            finally { con.Close(); }
            dra = ds.Tables["HighScores"].Rows;
            rowsrodraw = 1;
                      //todo add in for loop to increase [inc] to loop through each row upto say a max of x ammount 

        }
        public int testdbthing()
        {
            rowsrodraw = 1;
            
            rowsrodraw = ds.Tables["HighScores"].Rows.Count;

            return rowsrodraw;
        }
        public DataRowCollection test2()
        {
            dra = ds.Tables["HighScores"].Rows;
            return dra;
        }

        public void Draw(SpriteFont font,SpriteBatch sBatch,int count )
        {
            foreach (DataRow dRow in dra)
            {

                sBatch.DrawString(font, dRow[0].ToString(), new Vector2(5, 100 + count), Color.White);//draws the name 
                sBatch.DrawString(font, dRow[1].ToString(), new Vector2(200, 100 + count), Color.White);//draws the score 
                sBatch.DrawString(font, dRow[2].ToString(), new Vector2(300, 100 + count), Color.White);// draws the gamemode
                count += 50;
               // sBatch.End();
            }
        }
        public DbConn()
        {

        }
        public void addtoDB(string gamemode, int score)
        {
            try
            {
                con = new OleDbConnection(connectionString);
                con.Open();
            }
            catch (Exception ex)
            {
               // MessageBoxScreen message = new MessageBoxScreen("Error: Failed to create a database connection. \n{0}" + ex.Message, true);
                return;
            }

            OleDbCommand Cmd = new OleDbCommand("INSERT INTO HighScores(Score,Mode)", con);
            Cmd.Parameters.Add("@Score", OleDbType.VarNumeric, 20).Value = score;
            Cmd.Parameters.Add("@Mode", OleDbType.VarWChar, 20).Value = gamemode;

            try
            {
                Cmd.ExecuteNonQuery();
                MessageBoxScreen message = new MessageBoxScreen("Added Stuff", true);

            }
            catch (OleDbException ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBoxScreen message = new MessageBoxScreen("Error: Failed to add Data. \n{0}" + ex.Message, true);
                //MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }


        }
    }
}
