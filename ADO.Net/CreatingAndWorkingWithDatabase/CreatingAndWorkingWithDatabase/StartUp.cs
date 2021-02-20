using Microsoft.Data.SqlClient;
using ADO;
using System;

namespace CreatingAndWorkingWithDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {            
            string baseName = "MinionsDB";
            string DBCurrent = string.Format(DBCommands.DBCurrent, baseName);
            //InitialSetUp task1 = new InitialSetUp(baseName, DBCurrent);
            //task1.CreatingDatabase();
            //task1.CreatingTables();
            //task1.FillingDatabase();

            //VillainNames task2 = new VillainNames(DBCurrent);
            //Console.WriteLine(string.Join(Environment.NewLine, task2.Names()));
        }
    }
}
