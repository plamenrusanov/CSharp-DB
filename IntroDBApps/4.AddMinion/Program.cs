using IntroDBApps;
using System;
using System.Data.SqlClient;

namespace _4.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] minionsData = Console.ReadLine().Split();
            string[] villainData = Console.ReadLine().Split();

            string minionName = minionsData[1];
            int minionAge = int.Parse(minionsData[2]);
            string minionTown = minionsData[3];
            string villainName = villainData[1];

            using (SqlConnection dbCon = new SqlConnection(Configuration.ConnectionString))
            {
                dbCon.Open();
                int townId = GetTownId(minionTown, dbCon);
                int villainId = GetVillainId(villainName, dbCon);
                int minionId = InsertMinionAndGetId(minionName, minionAge, townId, dbCon);
                AssignMinionToVillain(minionId, villainId, dbCon);
                dbCon.Close();
            }
            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static void AssignMinionToVillain(int minionId, int villainId, SqlConnection dbCon)
        {
            string commandText = $"INSERT INTO MinionsVillains VALUES ({minionId}, {villainId})";
            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                command.ExecuteNonQuery();
            }
        }

        private static int InsertMinionAndGetId(string minionName, int minionAge, int townId, SqlConnection dbCon)
        {
            string commandText = $"INSERT INTO Minions VALUES ('{minionName}', {minionAge}, {townId})";

            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                command.ExecuteNonQuery();
            }

            commandText = $"SELECT Id FROM Minions WHERE Name = @name";

            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                command.Parameters.AddWithValue("@name", minionName);
                return (int)command.ExecuteScalar();
            }
        }

        private static int GetVillainId(string villainName, SqlConnection dbCon)
        {
            string commandText = $"select id from Villains where name = '{villainName}'";

            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                if (command.ExecuteScalar() == null)
                {
                    InsertVillain(villainName, dbCon);
                }
                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertVillain(string villainName, SqlConnection dbCon)
        {
            string commandText = $"INSERT INTO Villains VALUES ('{villainName}', 4)";
            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static int GetTownId(string minionTown, SqlConnection dbCon)
        {
            string commandText = $"select id from Towns where name = '{minionTown}'";

            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
                if (command.ExecuteScalar() == null)
                {
                    InsertTown(minionTown, dbCon);
                }
                return (int)command.ExecuteScalar();
            }        
        }

        private static void InsertTown(string minionTown, SqlConnection dbCon)
        {
            string commandText = $"INSERT INTO Towns (Name) VALUES ('{minionTown}')";
            using (SqlCommand command = new SqlCommand(commandText, dbCon))
            {
               // command.Parameters.AddWithValue("@Town", minionTown);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Town {minionTown} was added to the database.");
        }
    }
}
