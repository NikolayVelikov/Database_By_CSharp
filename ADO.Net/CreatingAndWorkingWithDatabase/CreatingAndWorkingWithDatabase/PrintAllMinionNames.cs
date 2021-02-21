using System.Text;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ADO
{
    public class PrintAllMinionNames
    {
        private readonly string dbCurrent = string.Empty;

        public PrintAllMinionNames(string dbCurrent)
        {
            this.dbCurrent = dbCurrent;
        }

        public string Run()
        {
            List<string> minions = new List<string>();
            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(DBCommands.allMinions,connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        if (name == null)
                        {
                            continue;
                        }
                        minions.Add(name);
                    }
                }

                connection.Close();
            }

            for (int i = 0; i <= (minions.Count - 1) / 2; i++)
            {
                string minion1 = minions[i];
                string minion2 = minions[(minions.Count - 1) - i];
                if (minion1 == minion2)
                {
                    sb.AppendLine(minion1);
                    continue;
                }

                sb.AppendLine(minion1);
                sb.AppendLine(minion2);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
