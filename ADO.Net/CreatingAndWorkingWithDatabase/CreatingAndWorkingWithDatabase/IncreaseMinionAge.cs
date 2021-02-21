using Microsoft.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class IncreaseMinionAge
    {
        private readonly string dbCurrent = string.Empty;
        private readonly string[] minionsId;

        public IncreaseMinionAge(string dbCurrent, string[] ids)
        {
            this.dbCurrent = dbCurrent;
            this.minionsId = ids;
        }

        public string Run()
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                for (int i = 0; i < this.minionsId.Length; i++)
                {
                    int id = int.Parse(minionsId[i]);
                    SqlCommand calculation = new SqlCommand(DBCommands.updateMinionsAgeAndUpperFirstLetter, connection);
                    calculation.Parameters.AddWithValue("@Number", id);
                    calculation.ExecuteScalar();
                }


                SqlCommand command = new SqlCommand(DBCommands.allMinionsWithNameAndAge, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int? age = (int?)reader["Age"];

                        sb.AppendLine(name + ' ' + age);
                    }
                }

                connection.Close();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
