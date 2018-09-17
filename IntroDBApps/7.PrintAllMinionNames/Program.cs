using IntroDBApps;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _7.PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> minions = new List<string>();
            using (SqlConnection dbcon = new SqlConnection(Configuration.ConnectionString))
            {
                dbcon.Open();

                string commandtext = "select Name from Minions";
                using (SqlCommand command = new SqlCommand(commandtext, dbcon))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add(reader[0].ToString());
                        }

                    }
                }
                dbcon.Close();
            }

            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[i]);
                Console.WriteLine(minions[minions.Count - i - 1]);
            }
            if (minions.Count % 2 == 1)
            {
                Console.WriteLine(minions[minions.Count / 2]);
            }
        }
    }
}
