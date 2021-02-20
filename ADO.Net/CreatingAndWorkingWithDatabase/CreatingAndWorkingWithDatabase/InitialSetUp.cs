using Microsoft.Data.SqlClient;

namespace ADO
{
    public class InitialSetUp
    {
        private string DBMaster = DBCommands.DBMaster;
        private string baseName = string.Empty;
        private string DBCurrent = string.Empty;

        public InitialSetUp(string dbName, string dbCurrent)
        {
            this.baseName = dbName;
            this.DBCurrent = dbCurrent;
        }

        public void CreatingDatabase()
        {
            using (var conncetion = new SqlConnection(DBMaster))
            {
                conncetion.Open();
                string createDB = "CREATE DATABASE " + this.baseName;
                ExecudeQuery(createDB, conncetion); // part 1

                conncetion.Close();
            }
        }
        public void CreatingTables() 
        {
            using (var conncetion = new SqlConnection(DBCurrent))
            {
                conncetion.Open();

                string[] tables = Tables(); // part 2
                foreach (string table in tables)
                {
                    ExecudeQuery(table, conncetion);
                }

                conncetion.Close();
            }
        }
        public void FillingDatabase()
        {
            using (var conncetion = new SqlConnection(DBCurrent))
            {
                conncetion.Open();

                string[] tableInformation = TableInformation(); // part 3
                foreach (string tableInfo in tableInformation)
                {
                    ExecudeQuery(tableInfo, conncetion);
                }

                conncetion.Close();
            }
        }

        private void ExecudeQuery(string query, SqlConnection connection)
        {
            using (var comand = new SqlCommand(query, connection))
            {
                comand.ExecuteNonQuery();
            }
        }

        private string[] TableInformation()
        {
            string[] result = new string[]
            {
                "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",
                "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",
                "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",
                "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",
                "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)",
                "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)"
            };

            return result;
        }

        private string[] Tables()
        {
            string[] result = new string[]
            {
                "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(250))",
                "CREATE TABLE Towns(	Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(250), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(250),	Age INT,	TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY,	Name NVARCHAR(250))",
                "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY,	Name NVARCHAR(250),	EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id),	VillainId INT FOREIGN KEY REFERENCES Villains(Id), PRIMARY KEY (MinionId,VillainId))"
            };

            return result;
        }
    }
}
