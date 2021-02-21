using Microsoft.Data.SqlClient;

namespace ADO
{
    public class IncreaseAgeStoredProcedure
    {
        private readonly string dbCurrent = string.Empty;

        public IncreaseAgeStoredProcedure(string dbCurrent)
        {
            this.dbCurrent = dbCurrent;
        }

        public void CreateStoredProcedure()
        {
            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(DBCommands.storedProcedure, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public string Run(int id)
        {
            string result = string.Empty;

            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(DBCommands.executionStoredProcedure, connection);
                command.Parameters.AddWithValue("@Id", id);
                result = (string)command.ExecuteScalar();

                connection.Close();
            }

            return result;
        }
    }
}
