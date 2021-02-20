using ADO;

namespace CreatingAndWorkingWithDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {            
            string baseName = "MinionsDB";
            string DBCurrent = string.Format(DBConncetion.DBCurrent, baseName);
            InitialSetUp task1 = new InitialSetUp(baseName, DBCurrent);
            task1.CreatingDatabase();
            task1.CreatingTables();
            task1.FillingDatabase();
        }
    }
}
