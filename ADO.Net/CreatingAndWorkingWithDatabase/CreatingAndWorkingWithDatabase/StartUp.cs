﻿using ADO;

namespace CreatingAndWorkingWithDatabase
{
    public class StartUp
    {
        // https://pastebin.com/k9HnYCV9
        public static void Main(string[] args)
        {
            string baseName = "MinionsDB";
            string dbCurrent = string.Format(DBCommands.DBCurrent, baseName);

            //InitialSetUp task1 = new InitialSetUp(baseName, dbCurrent);
            //task1.CreatingDatabase();
            //task1.CreatingTables();
            //task1.FillingDatabase();

            //VillainNames task2 = new VillainNames(dbCurrent);
            //Console.WriteLine(string.Join(Environment.NewLine, task2.Names()));


            //Console.Write("Fill the villain id: ");
            //int villainId = int.Parse(Console.ReadLine());
            //MinionNames task3 = new MinionNames(dbCurrent, villainId);
            //Console.WriteLine(task3.MinionsByVillain());

            //AddMinion task4 = new AddMinion(dbCurrent);
            //Console.WriteLine(task4.Run());

            //string countryName = Console.ReadLine();
            //ChangeTownNamesCasing task5 = new ChangeTownNamesCasing(dbCurrent, countryName);
            //Console.WriteLine(task5.Run());

            //PrintAllMinionNames task7 = new PrintAllMinionNames(dbCurrent);
            //Console.WriteLine(task7.Run());

            //string[] ids = Console.ReadLine().Split(' ',StringSplitOptions.RemoveEmptyEntries);
            //IncreaseMinionAge task8 = new IncreaseMinionAge(dbCurrent, ids);
            //Console.WriteLine(task8.Run());


            //IncreaseAgeStoredProcedure task9 = new IncreaseAgeStoredProcedure(dbCurrent);
            //task9.CreateStoredProcedure();
            //bool stop = true;
            //while (stop)
            //{
            //    int n;
            //    bool isNumeric = int.TryParse(Console.ReadLine(), out n);
            //    if (!isNumeric)
            //    {
            //        break;
            //    }

            //    Console.WriteLine(task9.Run(n));
            //}
        }
    }
}
