using System;
using System.Data.SqlClient;

namespace _3.MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-D1G8VKM\SQLEXPRESS;DATABASE=MinionsDB;Integrated Security=True";
            int villianId = int.Parse(Console.ReadLine());

            using (SqlConnection dbCon = new SqlConnection(connectionString))
            {
                dbCon.Open();
                string commandText = $"select Name from Villains where Id = {villianId}";

                SqlCommand command = new SqlCommand(commandText, dbCon);
                string villianName = (string)command.ExecuteScalar();
                if (villianName == null)
                {
                    Console.WriteLine($"No villain with ID {villianId} exists in the database.");
                }
                else
                {
                    commandText = $"select v.Name, m.Name, m.Age from Villains as v join MinionsVillains as mv on mv.VillainId = v.Id join Minions as m on m.Id = mv.MinionId where v.Id = 1 order by m.Name";
                    command = new SqlCommand(commandText, dbCon);
                    var resultReader =  command.ExecuteReader();
                    Console.WriteLine($"Villain: {villianName}");

                    if (!resultReader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        int row = 1;
                        while (resultReader.Read())
                        {
                            Console.WriteLine($"{row++}. {resultReader[1]} {resultReader[2]}");
                        }
                    }

                }


                dbCon.Close();
            }
        }
    }
}
