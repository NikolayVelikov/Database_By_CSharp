namespace ADO
{
    public static class DBCommands
    {
        public const string DBMaster = @"Data Source=DESKTOP-8F63LT6\TEW_SQLEXPRESS;Database=master;Integrated Security=true";
        public const string DBCurrent = @"Data Source=DESKTOP-8F63LT6\TEW_SQLEXPRESS;Database=" + "{0}" + ";Integrated Security=true";
        public const string villainNames = @"SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING  COUNT(mv.MinionId)  > 3 ORDER BY MinionsCount";

        public const string villainName = "SELECT [Name] FROM Villains WHERE id = {0}";
        public const string villainMinions = "SELECT ROW_NUMBER() OVER(ORDER BY m.Name) AS Sequence, m.Name, m.Age FROM MinionsVillains AS mv JOIN Minions AS m ON m.Id = mv.MinionId WHERE mv.VillainId = ";

        public const string minionTownId = "SELECT Id FROM Towns WHERE Name = @townName";
        public const string minionId = "SELECT Id FROM Minions WHERE Name = @Name AND Age = @Age";
        public const string creatingTwon = "INSERT INTO Towns(Name) VALUES(@townName)";
        public const string minionInformation = "SELECT m.Id AS MinionId, m.Name,m.Age,t.Id FROM Minions AS m LEFT JOIN Towns AS t ON t.Id = m.TownId WHERE m.Name = @Name AND m.Age = @Age AND t.id = @Id";
        public const string creatingMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
        public const string vilianId = "SELECT Id FROM Villains WHERE Name = @Name";
        public const string creatingVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
        public const string insertingMinionsToVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

        public const string findingCountryId = "SELECT Id FROM Countries WHERE Name = @Name";
        public const string updateTownNameToBeUpperCase = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @id";
        public const string allTownsByCountry = "SELECT t.Name FROM Towns AS t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Id = @Id";

        public const string allMinions = "SELECT Name FROM Minions WHERE Name IS NOT NULL";

        public const string updateMinionsAgeAndUpperFirstLetter = "UPDATE Minions SET Age = Age + 1, Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)) WHERE Id IN(@Number)";
        public const string allMinionsWithNameAndAge = "SELECT Name, Age FROM Minions WHERE Name IS NOT NULL";
    }
}
