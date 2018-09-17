using IntroDBApps;
using System;
using System.Data.SqlClient;

namespace _9.IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            int mId = int.Parse(Console.ReadLine());
            using (SqlConnection dbcon = new SqlConnection(Configuration.ConnectionString))
            {
                dbcon.Open();
                UpdateAge(mId, dbcon);
                PrintMinion(mId, dbcon);
                dbcon.Close();
            }
        }

        private static void PrintMinion(int mId, SqlConnection dbcon)
        {
            string commandText = $"select name, age from Minions where Id = {mId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                var reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine($"{reader[0]} – {reader[1]} years old");
            }
        }

        private static void UpdateAge(int mId, SqlConnection dbcon)
        {
            string commandText = $"exec usp_GetOlder {mId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
