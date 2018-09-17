using IntroDBApps;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5.ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using (SqlConnection dbcon = new SqlConnection(Configuration.ConnectionString))
            {
                dbcon.Open();
                int countryId = GetCountryId(countryName, dbcon);
                if (countryId == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    List<string> towns = UpdateAndGetTowns(countryId, dbcon);
                    if (towns.Count == 0)
                    {
                        Console.WriteLine("No town names were affected.");

                    }
                    else
                    {
                        Console.WriteLine($"{towns.Count} town names were affected.");
                        Console.WriteLine($"[{string.Join(", ", towns)}]");
                    }

                }
               
                dbcon.Close();
            }
        }

        private static List<string> UpdateAndGetTowns(int countryId, SqlConnection dbcon)
        {
            List<string> result = new List<string>();
            string commandText = $"UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = {countryId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                command.ExecuteNonQuery();
            }

            commandText = $"SELECT Name FROM Towns WHERE CountryCode = {countryId}";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
            }
            return result;
        }

        private static int GetCountryId(string countryName, SqlConnection dbcon)
        {
            string commandText = $"SELECT Id FROM Countries where Name = '{countryName}'";
            using (SqlCommand command = new SqlCommand(commandText, dbcon))
            {
                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }
                else
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
