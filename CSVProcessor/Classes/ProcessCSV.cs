using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace CSVProcessor.Classes
{

    public class ProcessCSV
    {
        //each element of CSV file
        private string filePath;
        private List<string> number = new List<string>();
        private List<string> date = new List<string>();
        private List<string> account = new List<string>();
        private List<string> ammount = new List<string>();
        private List<string> subCategory = new List<string>();
        private List<string> memo = new List<string>();

        //Reference numbers from database
        List<string> DBList = new List<string>();

        public ProcessCSV(string pathInp)
        {
            filePath = pathInp;
            processFile();
            getDBMembers();
        }

        //extracts individual elements from each line
        private void processFile()
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    number.Add(values[0]);
                    date.Add(values[1]);
                    account.Add(values[2]);
                    ammount.Add(values[3]);
                    subCategory.Add(values[4]);
                    memo.Add(values[5]);
                }
            }
        }

        //gets referenc numbers from database
        private void getDBMembers()
        {
            DBClass.openConnection();

            DBClass.sql = "SELECT [Membership Reference] FROM Monthly";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            using (DBClass.rd = DBClass.cmd.ExecuteReader())
            {
                while (DBClass.rd.Read())
                {
                    DBList.Add(DBClass.rd.GetValue(0).ToString());
                }
            }

            DBClass.closeConnection();
        }

        //compares reference numbers between DB and CSV file to filter numbers not present within CSV file
        private List<string> filterMembers()
        {
            List<string> notPaid = new List<string>();
            List<string> splitDBList = new List<string>();
            for (int i = 0; i < DBList.Count; i++)
            {
                var element = DBList[i];
                var elementSplit = element.Split('/');
                splitDBList.Add(elementSplit[1]);
            }

            for (int i = 0; i < splitDBList.Count; i++)
            {
                bool found = false;
                for (int x = 0; x < memo.Count; x++)
                {
                    if (memo[x].Contains(splitDBList[i]))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    notPaid.Add(DBList[i]);
                }
            }

            return notPaid;
        }

        //retrieves names on records with reference numbers 
        public List<string> getMembers()
        {
            List<string> unpaidMembers = new List<string>();
            List<string> referenceList = filterMembers();


            for (int i = 0; i < referenceList.Count; i++)
            {

                using (SqlConnection conn = DBClass.con)
                {
                    DBClass.openConnection();
                    string sql = "SELECT [Name] FROM Monthly WHERE [Membership Reference] LIKE @myVar";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@myVar", referenceList[i]);

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            unpaidMembers.Add(rd.GetValue(0).ToString());
                        }
                    }
                    DBClass.closeConnection();
                }
            }

            return unpaidMembers;
        }

    }
}

