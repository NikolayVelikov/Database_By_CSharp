using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ADO
{
    class VillainNames
    {
        private string dbCurrent = string.Empty;

        public VillainNames(string dbCurrent)
        {
            this.dbCurrent = dbCurrent;
        }

        public List<string> Names()
        {
            List<string> names = new List<string>();

            using (SqlConnection connection = new SqlConnection(dbCurrent))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(DBCommands.villainNames, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int counts = (int)reader["MinionsCount"];

                        string info = name + Symbols.outputTask2 + counts;
                        names.Add(info);
                    }
                }

                connection.Close();
            }

            return names;
        }
    }
}
