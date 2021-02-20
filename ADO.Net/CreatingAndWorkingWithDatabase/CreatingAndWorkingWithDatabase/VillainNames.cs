using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADO
{
    class VillainNames
    {
        private string DBCurrent = string.Empty;
        public VillainNames(string dbCurrent)
        {
            this.DBCurrent = dbCurrent;
        }

        public List<string> Names()
        {
            List<string> names = new List<string>();

            using (SqlConnection connection = new SqlConnection(DBCurrent))
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
