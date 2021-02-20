using System.Text;
using Microsoft.Data.SqlClient;

namespace ADO
{
    public class MinionNames
    {
        private readonly string dbCurrent = string.Empty;
        private readonly int? villainId = null;

        public MinionNames(string dbCurrent, int villainId)
        {
            this.dbCurrent = dbCurrent;
            this.villainId = villainId;
        }

        public string MinionsByVillain()
        {
            bool villainExist = true;
            StringBuilder sb = new StringBuilder();
            using (SqlConnection connection = new SqlConnection(dbCurrent))
            {
                connection.Open();

                SqlCommand commandVillain = new SqlCommand(string.Format(DBCommands.villainName, this.villainId), connection);
                string villainName = (string)commandVillain.ExecuteScalar();
                if (villainName == null)
                {
                    sb.AppendLine(string.Format(OutputMessages.NoVillain, this.villainId));
                    villainExist = false;
                }

                if (villainExist)
                {
                    sb.AppendLine(string.Format(OutputMessages.VillainName, villainName));
                    string query = DBCommands.villainMinions + this.villainId;
                    SqlCommand commandMinions = new SqlCommand(query, connection);
                    SqlDataReader reader = commandMinions.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        sb.AppendLine(OutputMessages.noMinionsByVillain);
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            long sequance = (long)reader["Sequence"];
                            string minionName = (string)reader["Name"];
                            int age = (int)reader["Age"];

                            string rowResult = string.Format(OutputMessages.minionsByVillain, sequance, minionName, age);
                            sb.AppendLine(rowResult);
                        }
                    }
                }
                connection.Close();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
