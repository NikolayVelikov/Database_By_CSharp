using System;
using System.Text;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace ADO
{
    public class AddMinion
    {
        private readonly string dbCurrent = string.Empty;

        public AddMinion(string dbCurrent)
        {
            this.dbCurrent = dbCurrent;
        }

        public string Run()
        {
            string[] minionInformation = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            string minionName = minionInformation[1];
            int minionAge = int.Parse(minionInformation[2]);
            string minionTownName = minionInformation[3];
            int? minionId = -1;
            int? townId = -1;

            string[] villainInformation = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            string villainName = villainInformation[1];
            int? villainId = -1;

            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.dbCurrent))
            {
                connection.Open();

                using (SqlCommand comand = new SqlCommand(DBCommands.minionTownId, connection))
                {
                    comand.Parameters.AddWithValue("@townName", minionTownName);
                    townId = (int?)comand.ExecuteScalar();

                    if (townId == null)
                    {
                        SqlCommand creatingTwon = new SqlCommand(DBCommands.creatingTwon, connection);
                        creatingTwon.Parameters.AddWithValue("@townName", minionTownName);
                        creatingTwon.ExecuteNonQuery();

                        townId = (int)comand.ExecuteScalar();
                        sb.AppendLine(string.Format(OutputMessages.townIdAdded, minionTownName));
                    }
                }

                using (SqlCommand minionComand = new SqlCommand(DBCommands.minionInformation, connection))
                {
                    minionComand.Parameters.AddWithValue("@Name", minionName);
                    minionComand.Parameters.AddWithValue("@Age", minionAge);
                    minionComand.Parameters.AddWithValue("@Id", townId);
                    bool minionExist = false;
                    using (SqlDataReader reader = minionComand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            reader.Close();
                            minionId = CreatingMinion(minionName, minionAge, connection, townId);
                            minionExist = true;
                        }
                        else
                        {

                            while (reader.Read())
                            {
                                string name = (string)reader["Name"];
                                int? age = (int?)reader["Age"];
                                int? id = (int?)reader["Id"];
                                if (name == minionName && age == minionAge && id == townId)
                                {
                                    minionExist = true;
                                    minionId = (int)reader["MinionId"];
                                    break;
                                }
                            }

                            if (!minionExist)
                            {
                                reader.Close();
                                minionId = CreatingMinion(minionName, minionAge, connection, townId);
                                minionExist = true;
                            }
                        }
                    }
                }
                villainId = VillainId(villainName, connection);
                if (villainId == null)
                {
                    CreatingVillain(villainName, connection);

                    villainId = VillainId(villainName, connection);
                    sb.AppendLine(string.Format(OutputMessages.addingNewVillian, villainName));

                    InsertingMinionToVillain(minionName, minionId, villainName, villainId, sb, connection);
                }
                else
                {
                    InsertingMinionToVillain(minionName, minionId, villainName, villainId, sb, connection);
                }

                connection.Close();
            }

            return sb.ToString().TrimEnd();
        }

        private void CreatingVillain(string villainName, SqlConnection connection)
        {
            SqlCommand villainComand = new SqlCommand(DBCommands.creatingVillain, connection);
            villainComand.Parameters.AddWithValue("@villainName", villainName);
            villainComand.ExecuteNonQuery();
        }

        private void InsertingMinionToVillain(string minionName, int? minionId, string villainName, int? villainId, StringBuilder sb, SqlConnection connection)
        {
            SqlCommand insertMiniontoVillain = new SqlCommand(DBCommands.insertingMinionsToVillain, connection);
            insertMiniontoVillain.Parameters.AddWithValue("@villainId", villainId);
            insertMiniontoVillain.Parameters.AddWithValue("@minionId", minionId);
            sb.AppendLine(string.Format(OutputMessages.finallyForAddingMinons, minionName, villainName));
        }

        private int? VillainId(string villainName, SqlConnection connection)
        {
            SqlCommand villainComand = new SqlCommand(DBCommands.vilianId, connection);
            villainComand.Parameters.AddWithValue("@Name", villainName);

            int? id = (int?)villainComand.ExecuteScalar();

            return id;
        }

        private int CreatingMinion(string minionName, int minionAge, SqlConnection connection, int? townId)
        {
            SqlCommand comand = new SqlCommand(DBCommands.creatingMinion, connection);
            comand.Parameters.AddWithValue("@name", minionName);
            comand.Parameters.AddWithValue("@age", minionAge);
            comand.Parameters.AddWithValue("@townId", townId);
            comand.ExecuteNonQuery();

            //SELECT Id FROM Minions WHERE Name = @Name AND Age = @Age
            comand = new SqlCommand(DBCommands.minionId, connection);
            comand.Parameters.AddWithValue("@Name", minionName);
            comand.Parameters.AddWithValue("@Age", minionAge);
            int id = (int)comand.ExecuteScalar();

            return id;
        }
    }
}
