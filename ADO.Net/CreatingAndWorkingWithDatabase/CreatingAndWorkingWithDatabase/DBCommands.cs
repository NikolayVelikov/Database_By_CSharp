namespace ADO
{
    public static class DBCommands
    {
        public const string DBMaster = @"Data Source=DESKTOP-8F63LT6\TEW_SQLEXPRESS;Database=master;Integrated Security=true";
        public const string DBCurrent = @"Data Source=DESKTOP-8F63LT6\TEW_SQLEXPRESS;Database=" + "{0}" + ";Integrated Security=true";
        public const string villainNames = "SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING  COUNT(mv.MinionId)  >= 3 ORDER BY MinionsCount";
    }
}
