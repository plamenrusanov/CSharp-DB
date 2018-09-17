using IntroDBApps;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace _8.IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] minionsId = Console.ReadLine().Split().Select(int.Parse).ToArray();
            using (SqlConnection dbcon = new SqlConnection(Configuration.ConnectionString))
            {
                dbcon.Open();
                foreach (int mId in minionsId)
                {
                    UpdateAge(mId, dbcon);
                    CaseName(mId, dbcon);
                }
                PrintMinions(dbcon);
                dbcon.Close();
            }
        }

        private static void PrintMinions(SqlConnection dbcon)
        {
            string commandText = $"select Name, age from Minions";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static void CaseName(int mId, SqlConnection dbcon)
        {
            string commandText = $"UPDATE Minions SET Name = UPPER(LEFT(Name, 1)) + LOWER(SUBSTRING(Name, 2, LEN(Name))) where id = {mId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void UpdateAge(int mId, SqlConnection dbcon)
        {
            string commandText = $"update Minions set Age += 1 where Id = {mId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
