using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;


namespace GradedUnit.Screens
{
    class HighScoreScreen
    {
        public void loadDb()
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:/Documents/GitHub/gradunit/GradedUnit/HighScores.mdb";
            string strSelect = "SELECT        HighScores.Score, HighScores.ModeType, Player.Name FROM            (HighScores INNER JOIN Player ON HighScores.[user id] = Player.ID AND HighScores.[user id] = Player.ID)";
            DataSet myDataSet = new DataSet();
            OleDbConnection con = null;
            try
            {
                con = new OleDbConnection(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to create a database connection. \n{0}", ex.Message);
                return;

            }
        }

        //HighScoreScreen();
    }
}
