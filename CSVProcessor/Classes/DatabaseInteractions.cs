using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;


namespace CSVProcessor.Classes
{
    class DBClass
    {
        //retrieves connection string from App.config
        public static string GetConnectionStrings()
        {
            string strConString = ConfigurationManager.ConnectionStrings["conString"].ToString();
            return strConString;
        }

        public static string sql;
        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmd = new SqlCommand("", con);
        public static SqlDataReader rd;
        public static DataTable dt;
        public static SqlDataAdapter da;

        //opens connection to database
        public static void openConnection()
        {
            string strConString = ConfigurationManager.ConnectionStrings["conString"].ToString();

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = GetConnectionStrings();
                    con.Open();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("The system failed to establish a connection to " + strConString + "." + Environment.NewLine + "Descriptions: " + ex.Message.ToString(), "Bawtry Gym Server", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //closes connection to database
        public static void closeConnection()
        {
            string strConString = ConfigurationManager.ConnectionStrings["conString"].ToString();
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The system failed to close a connection to " + strConString + "." + Environment.NewLine + "Descriptions: " + ex.Message.ToString(), "Bawtry Gym Server", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

    }
}
