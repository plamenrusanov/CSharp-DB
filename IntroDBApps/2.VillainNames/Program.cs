using System;
using System.Data.SqlClient;

namespace _2.VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-D1G8VKM\SQLEXPRESS;DATABASE=MinionsDB;Integrated Security=True";
            using(SqlConnection dbCon = new SqlConnection(connectionString))
            {
                dbCon.Open();
                string commandText = @"select v.Name, count(m.Id) as [minions] from Villains as v " + 
                @"join MinionsVillains as mv on mv.VillainId = v.Id join Minions as m on m.Id = mv.MinionId "+
                @"group by v.Name having count(m.Id) > 3 order by minions desc";
                SqlCommand command = new SqlCommand(commandText, dbCon);
                var reader = command.ExecuteReader();
              

                while (reader.Read())
                {
                    string viliiainName = (string)reader[0];
                    int countMinions = (int)reader[1];
                    Console.WriteLine($"{viliiainName} - {countMinions}");
                }
                dbCon.Close();
            }
        }
    }
}
