using ADO;

namespace CreatingAndWorkingWithDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string baseName = "MinionsDB";
            InitialSetUp task1 = new InitialSetUp(baseName);
            task1.Run();
        }
    }
}
