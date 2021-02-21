using System.Text;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ADO
{
    public class ChangeTownNamesCasing
    {
        private readonly string dbCurrent = string.Empty;
        private readonly string countryName = string.Empty;

        public ChangeTownNamesCasing(string dbCurrent, string name)
        {
            this.dbCurrent = dbCurrent;
            this.countryName = name;
        }

        public string Run()
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(DBCommands.findingCountryId, connection);
                command.Parameters.AddWithValue("@Name", this.countryName);
                int? countryId = (int?)command.ExecuteScalar();

                if (countryId == null)
                {
                    sb.Append(OutputMessages.noAffectedTowns);
                    connection.Close();
                    return sb.ToString().TrimEnd();
                }

                command = new SqlCommand(DBCommands.updateTownNameToBeUpperCase, connection);
                command.Parameters.AddWithValue("@Id", countryId);
                int affected = command.ExecuteNonQuery();

                if (affected == 0)
                {
                    sb.Append(OutputMessages.noAffectedTowns);
                    connection.Close();
                    return sb.ToString().TrimEnd();
                }

                sb.AppendLine(string.Format(OutputMessages.affectedTowns, affected));
                
                command = new SqlCommand(DBCommands.allTownsByCountry, connection);
                command.Parameters.AddWithValue("@Id", countryId);
                using (SqlDataReader reader = command.ExecuteReader())
                {                    
                    List<string> towns = new List<string>();
                    while (reader.Read())
                    {
                        towns.Add((string)reader["Name"]);
                    }
                    sb.AppendLine('[' + string.Join(", ", towns) + ']');
                }

                connection.Close();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
