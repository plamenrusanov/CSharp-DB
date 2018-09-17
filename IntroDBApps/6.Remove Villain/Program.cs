using IntroDBApps;
using System;
using System.Data.SqlClient;

namespace _6.RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection dbcon = new SqlConnection(Configuration.ConnectionString))
            {
                dbcon.Open();
                SqlTransaction transaction = dbcon.BeginTransaction();

                try
                {
                    string villainName = GetNameVillain(villainId, dbcon, transaction);
                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                    }
                    else
                    {
                        int releaseMinions = ReleaseMinions(villainId, dbcon, transaction);
                        DeleteVillain(villainId, dbcon, transaction);
                        Console.WriteLine($"{villainName} was deleted.");
                        Console.WriteLine($"{releaseMinions} minions were released.");
                    }
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                }
                transaction.Commit();
                dbcon.Close();
            }
        }

        private static void DeleteVillain(int villainId, SqlConnection dbcon, SqlTransaction transaction)
        {
            string commandText = $"delete from Villains where Id = {villainId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon, transaction))
            {
                command.ExecuteNonQuery();
            }
        }

        private static int ReleaseMinions(int villainId, SqlConnection dbcon, SqlTransaction transaction)
        {
            string commandText = $"delete from MinionsVillains where VillainId = {villainId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon, transaction))
            {
                return command.ExecuteNonQuery();
            }
        }

        private static string GetNameVillain(int villainId, SqlConnection dbcon, SqlTransaction transaction)
        {
            string commandText = $"SELECT Name FROM Villains WHERE Id = {villainId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon, transaction))
            {
                return (string)command.ExecuteScalar();
            }
        }
    }
}
