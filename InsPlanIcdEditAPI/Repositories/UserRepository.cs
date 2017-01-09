using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;

namespace InsPlanIcdEditApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(string sqlDbConnectionString)
        {
            SqlDbConnectionString = sqlDbConnectionString;
        }
        private string SqlDbConnectionString = string.Empty;

        public UserObject GetUser(string username)
        {
            UserObject data = null;
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetUser";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = username;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                data = MapReaderToUserObject(reader);
            }
            return data;
        }

        private UserObject MapReaderToUserObject(SqlDataReader reader)
        {
            UserObject data = null;
            if (reader != null && !reader.IsClosed && reader.Read())
            {
                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                var username = reader.GetString(reader.GetOrdinal("Username"));
                var firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                var lastName = reader.GetString(reader.GetOrdinal("LastName"));
                var isAuthorized = reader.GetInt32(reader.GetOrdinal("IsAuthorized"));
 
                data = new UserObject()
                {
                    id = id,
                    username = username,
                    firstName = firstName,
                    lastName = lastName,
                    isAuthorized = isAuthorized == 1 ? true : false
                };

                reader.Close();
            }
            return data;
        }
    }
}