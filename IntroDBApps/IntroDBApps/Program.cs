using System;
using System.Data.SqlClient;

namespace IntroDBApps
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-D1G8VKM\SQLEXPRESS;Integrated Security=True";

            using(SqlConnection dbCon = new SqlConnection(connectionString))
            {
                dbCon.Open();
                string commandText = "create database MinionsDB";
                Comm(dbCon, commandText);

                commandText = "use MinionsDB";
                Comm(dbCon, commandText);
               //dbCon.ChangeDatabase("MinionsDB");

                commandText = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";
                Comm(dbCon, commandText);

                commandText = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";
                Comm(dbCon, commandText);

                commandText = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";
                Comm(dbCon, commandText);

                commandText = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";
                Comm(dbCon, commandText);

                commandText = "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";
                Comm(dbCon, commandText);

                commandText = "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO Countries([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";
                Comm(dbCon, commandText);

                commandText = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";
                Comm(dbCon, commandText);
                dbCon.Close();
            }
        }

        private static void Comm(SqlConnection dbCon, string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, dbCon);
            command.ExecuteNonQuery();
        }
    }
}
