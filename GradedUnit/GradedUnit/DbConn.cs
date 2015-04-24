using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
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

        DataRowCollection dra;//datarowcollection 
        int rowsrodraw = 0; //howmanyrows to draw 
        DataSet dataset = new DataSet();//creates a new dataset 
        OleDbConnection con = null;//creates a new connection value 
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:/Documents/GitHub/gradunit/GradedUnit/HighScores.mdb";//the position as to where the database is on the filesystem 
        OleDbTransaction trans = null;//sets the default value of the transaction ot be null 
        int id;//integer for id 

     // loads teh database 
        public void loadDb(string mode)
        {
            dataset.Clear();//empties the dataset 
            string strSelect;
            if (mode == "CoOp") // the gamemode is coop load the coop database if not load the comp database 
            {
                strSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user_id] = Player.ID AND HighScores.[user_id] = Player.ID) WHERE (HighScores.ModeType = 'CoOp') ORDER BY HighScores.Score DESC, Player.Name;";
            }
            else { strSelect = "SELECT Player.Name, HighScores.Score, HighScores.ModeType FROM (HighScores INNER JOIN Player ON HighScores.[user_id] = Player.ID AND HighScores.[user_id] = Player.ID) WHERE (HighScores.ModeType = 'Comp') ORDER BY HighScores.Score DESC, Player.Name;"; }
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
                OleDbCommand accessCmd = new OleDbCommand(strSelect, con);//SETS COMMAND TO USE THE QUERY UP ABOVE 
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(accessCmd);//SETS THE DATAADAPTER TO USE THE COMMAND AND CONNECTION 
    
                con.Open();
                dataAdapter.Fill(dataset, "HighScores");
            }
            catch (Exception ex)
            { MessageBoxScreen message = new MessageBoxScreen("Error: Failed to retrieve the data . \n{0}" + ex.Message, true); }
            finally { con.Close(); }
            dra = dataset.Tables["HighScores"].Rows;//maps teh valuse selected to the data collection 
            rowsrodraw = 1;//sets rows to draw to one 

        }
     //check how many rows are in the the database which is loaded 
        public int checkRows()
        {
            rowsrodraw = 1;

            rowsrodraw = dataset.Tables["HighScores"].Rows.Count;

            return rowsrodraw;
        }
     //draws the rows on the database 
        public void Draw(SpriteFont font,SpriteBatch sBatch,int count )
        {
            foreach (DataRow dRow in dra)
            {

                sBatch.DrawString(font, dRow[0].ToString(), new Vector2(5, 100 + count), Color.White);//draws the name 
                sBatch.DrawString(font, dRow[1].ToString(), new Vector2(250, 100 + count), Color.White);//draws the score 
                sBatch.DrawString(font, dRow[2].ToString(), new Vector2(350, 100 + count), Color.White);// draws the gamemode
                count += 50;
               // sBatch.End();
            }
        }
        public DbConn()
        {

        }
     //adds the value game mode and score to the highscores database 
        public void addtoDB(string gamemode,int score)
        {
            try
            {
                con = new OleDbConnection(connectionString);
                con.Open(); // open connection 
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
                return;
            }

                OleDbCommand Cmd = new OleDbCommand();//("INSERT INTO HighScores(Score,ModeType) VALUES(@Score,@Mode);", con);
                trans = con.BeginTransaction();

                   
                    Cmd.Connection = con;
                   Cmd.Transaction = trans;
                   try {
                       Debug.WriteLine(id.GetType());
                   Cmd.CommandText = "INSERT INTO HighScores(Score,ModeType,user_id) VALUES(@Score,@Mode,@id);";//SQL SELECT QYERY 
                   Cmd.Parameters.AddWithValue("@Score", score);//VALUES ADDED 
                  Cmd.Parameters.AddWithValue("@Mode", gamemode);//VALUES ADDED 
                  Cmd.Parameters.AddWithValue("@id", id);//VALUES ADDED 
                  Debug.WriteLine(id);
                   Cmd.ExecuteNonQuery();//EXECUTE COMMAND 
                   trans.Commit();//COMMIT 
                   }
                    catch(Exception ex)
                   {
                        MessageBox.Show(ex.ToString());
                   }

                con.Close();


        }
     //adds the value name to the player database 
        public void addNametoDb(string Name)
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
            try
            {

                OleDbCommand Cmd = new OleDbCommand();//("INSERT INTO HighScores(Score,ModeType) VALUES(@Score,@Mode);", con);
                Cmd.Connection = con;//CONNECTION STRING 
                trans = con.BeginTransaction();//STARTS THE TRANSACTION 
                Cmd.Transaction = trans;
                Cmd.CommandText = "INSERT INTO Player(Name) VALUES(@Name);";//SQL QUERY 
                Cmd.Parameters.AddWithValue("@Name", Name);//VALUES USED 
                Cmd.ExecuteNonQuery();//START THE QUERY 
                Cmd.CommandText = "SELECT @@IDENTITY;";//GETTHE AUTOGENNED ID OFTHE PLAYER 
                id = (int)Cmd.ExecuteScalar();//SET IT TO ID 
                trans.Commit();//COMMIT 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            con.Close();
        }
    }
}
