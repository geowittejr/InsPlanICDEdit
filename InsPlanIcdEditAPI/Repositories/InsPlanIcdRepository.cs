using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Repositories
{
    public class InsPlanIcdRepository : IInsPlanIcdRepository
    {
        public InsPlanIcdRepository(string sqlDbConnectionString)
        {
            SqlDbConnectionString = sqlDbConnectionString;
        }
        private string SqlDbConnectionString = string.Empty;

        public List<InsPlan> GetInsPlans(string filterText, int startIndex, int endIndex, 
            string sortColumn, bool sortDesc, out int totalPlans)
        {
            //Get all insurance plans
            var returnObj = new Dictionary<string, object>();
            var data = new List<InsPlan>();
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetInsPlans";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@FilterText", System.Data.SqlDbType.NVarChar).Value = filterText;
                cmd.Parameters.Add("@StartIndex", System.Data.SqlDbType.Int).Value = startIndex;
                cmd.Parameters.Add("@EndIndex", System.Data.SqlDbType.Int).Value = endIndex;
                cmd.Parameters.Add("@SortColumn", System.Data.SqlDbType.NVarChar).Value = sortColumn;
                cmd.Parameters.Add("@SortDesc", System.Data.SqlDbType.Bit).Value = sortDesc ? 1 : 0;
                var totPlansParm = cmd.Parameters.Add("@TotalPlans", System.Data.SqlDbType.Int);
                totPlansParm.Direction = System.Data.ParameterDirection.Output;
                
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);
                data = MapReaderToInsPlanList(reader, false);

                int total = 0;
                int.TryParse(totPlansParm.Value.ToString(), out total);
                totalPlans = total;
            }
            return data;
        }

        public InsPlan GetInsPlan(string planId)
        {
            InsPlan data = null;            
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetInsPlan";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlanId", System.Data.SqlDbType.NVarChar).Value = planId;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                data = MapReaderToInsPlan(reader);
            }            
            return data;
        }

        public List<Icd9> GetInsPlanIcd9s(string planId, string filterText, int startIndex, int endIndex, 
            string status, string sortColumn, bool sortDesc, out int totalIcd9s)
        {
            //Get Icd9s for the specified insurance plan
            var data = new List<Icd9>();
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetInsPlanIcd9s";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlanId", System.Data.SqlDbType.NVarChar).Value = planId;
                cmd.Parameters.Add("@FilterText", System.Data.SqlDbType.NVarChar).Value = filterText;
                cmd.Parameters.Add("@StartIndex", System.Data.SqlDbType.Int).Value = startIndex;
                cmd.Parameters.Add("@EndIndex", System.Data.SqlDbType.Int).Value = endIndex;
                cmd.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar).Value = status;
                cmd.Parameters.Add("@SortColumn", System.Data.SqlDbType.NVarChar).Value = sortColumn;
                cmd.Parameters.Add("@SortDesc", System.Data.SqlDbType.Bit).Value = sortDesc ? 1 : 0;
                var totIcd9sParm = cmd.Parameters.Add("@TotalIcd9s", System.Data.SqlDbType.Int);
                totIcd9sParm.Direction = System.Data.ParameterDirection.Output;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);
                data = MapReaderToIcd9List(reader, true);

                int total = 0;
                int.TryParse(totIcd9sParm.Value.ToString(), out total);
                totalIcd9s = total;
            }
            return data;
        }

        public bool AddInsPlanIcd9(string planId, string icd9, string username)
        {
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_AddInsPlanIcd9";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlanId", System.Data.SqlDbType.NVarChar).Value = planId;
                cmd.Parameters.Add("@Icd9", System.Data.SqlDbType.NVarChar).Value = icd9;
                cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = username;
                connection.Open();

                try
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                }
                catch { return false; }
            }
            return true;
        }

        public bool RemoveInsPlanIcd9(string planId, string icd9, string username)
        {
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_RemoveInsPlanIcd9";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlanId", System.Data.SqlDbType.NVarChar).Value = planId;
                cmd.Parameters.Add("@Icd9", System.Data.SqlDbType.NVarChar).Value = icd9;
                cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = username;
                connection.Open();

                try
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                }
                catch { return false; }
            }
            return true;
        }

        public List<Icd9> GetIcd9s(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalIcd9s)
        {
            //Get all icd9 codes
            var data = new List<Icd9>();
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetIcd9s";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@FilterText", System.Data.SqlDbType.NVarChar).Value = filterText;
                cmd.Parameters.Add("@StartIndex", System.Data.SqlDbType.Int).Value = startIndex;
                cmd.Parameters.Add("@EndIndex", System.Data.SqlDbType.Int).Value = endIndex;
                cmd.Parameters.Add("@SortColumn", System.Data.SqlDbType.NVarChar).Value = sortColumn;
                cmd.Parameters.Add("@SortDesc", System.Data.SqlDbType.Bit).Value = sortDesc ? 1 : 0;
                var totIcd9sParm = cmd.Parameters.Add("@TotalIcd9s", System.Data.SqlDbType.Int);
                totIcd9sParm.Direction = System.Data.ParameterDirection.Output;
                
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);
                
                data = MapReaderToIcd9List(reader, false);

                int total = 0;
                int.TryParse(totIcd9sParm.Value.ToString(), out total);
                totalIcd9s = total;
            }
            return data;
        }

        public Icd9 GetIcd9(string icd9)
        {
            Icd9 data = null;
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetIcd9";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Icd9", System.Data.SqlDbType.NVarChar).Value = icd9;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                data = MapReaderToIcd9(reader);
            }
            return data;
        }

        public List<InsPlan> GetIcd9InsPlans(string icd9, string filterText, int startIndex, int endIndex, string status,
            string sortColumn, bool sortDesc, out int totalPlans)
        {
            //Get a list of insurance plans for the specified icd9
            var data = new List<InsPlan>();
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetIcd9InsPlans";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Icd9", System.Data.SqlDbType.NVarChar).Value = icd9;
                cmd.Parameters.Add("@FilterText", System.Data.SqlDbType.NVarChar).Value = filterText;
                cmd.Parameters.Add("@StartIndex", System.Data.SqlDbType.Int).Value = startIndex;
                cmd.Parameters.Add("@EndIndex", System.Data.SqlDbType.Int).Value = endIndex;
                cmd.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar).Value = status;
                cmd.Parameters.Add("@SortColumn", System.Data.SqlDbType.NVarChar).Value = sortColumn;
                cmd.Parameters.Add("@SortDesc", System.Data.SqlDbType.Bit).Value = sortDesc ? 1 : 0;
                var totPlansParm = cmd.Parameters.Add("@TotalPlans", System.Data.SqlDbType.Int);
                totPlansParm.Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);
                data = MapReaderToInsPlanList(reader, true);

                int total = 0;
                int.TryParse(totPlansParm.Value.ToString(), out total);
                totalPlans = total;
            }
            return data;
        }

        public void PopulateIcd9CodesTable()
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_PopulateIcd9CodesTable";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
        }

        public void PopulateInsPlansTable()
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_PopulateInsPlansTable";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
        }

        #region Mappers 

        private List<InsPlan> MapReaderToInsPlanList(SqlDataReader reader, bool includeIcd9Status)
        {
            var data = new List<InsPlan>();
            InsPlan plan = null;
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    var planId = reader.GetString(reader.GetOrdinal("InsPlanId"));
                    var insCoId = reader.GetString(reader.GetOrdinal("InsCoId"));
                    var insCoDesc = reader.GetString(reader.GetOrdinal("InsCoDesc"));
                    var icd9Count = reader.GetInt32(reader.GetOrdinal("Icd9Count"));
                    var insCoIdAlt = !reader.IsDBNull(reader.GetOrdinal("InsCoIdAlternate")) ? reader.GetString(reader.GetOrdinal("InsCoIdAlternate")) : null;
                    var insCoDescAlt = !reader.IsDBNull(reader.GetOrdinal("InsCoDescAlternate")) ? reader.GetString(reader.GetOrdinal("InsCoDescAlternate")) : null;
                    var enabledOnIcd9 = includeIcd9Status ? reader.GetInt32(reader.GetOrdinal("EnabledOnIcd9")) : 0;

                    plan = new InsPlan()
                    {
                        planId = planId,
                        insCoId = insCoId,
                        insCoDesc = insCoDesc,
                        icd9Count = icd9Count,
                        insCoIdAlt = insCoIdAlt,
                        insCoDescAlt = insCoDescAlt,
                        enabledOnIcd9 = enabledOnIcd9 == 1 ? true : false
                    };
                    data.Add(plan);
                }
                reader.Close();
            }
            return data;
        }

        private InsPlan MapReaderToInsPlan(SqlDataReader reader)
        {
            InsPlan data = null;
            if (reader != null && !reader.IsClosed && reader.Read())
            {
                var planId = reader.GetString(reader.GetOrdinal("InsPlanId"));
                var insCoId = reader.GetString(reader.GetOrdinal("InsCoId"));
                var insCoDesc = reader.GetString(reader.GetOrdinal("InsCoDesc"));
                var icd9Count = reader.GetInt32(reader.GetOrdinal("Icd9Count"));
                var insCoIdAlt = !reader.IsDBNull(reader.GetOrdinal("InsCoIdAlternate")) ? reader.GetString(reader.GetOrdinal("InsCoIdAlternate")) : null;
                var insCoDescAlt = !reader.IsDBNull(reader.GetOrdinal("InsCoDescAlternate")) ? reader.GetString(reader.GetOrdinal("InsCoDescAlternate")) : null;
                
                data = new InsPlan()
                {
                    planId = planId,
                    insCoId = insCoId,
                    insCoDesc = insCoDesc,
                    icd9Count = icd9Count,
                    insCoIdAlt = insCoIdAlt,
                    insCoDescAlt = insCoDescAlt
                };

                reader.Close();
            }
            return data;
        }

        private List<Icd9> MapReaderToIcd9List(SqlDataReader reader, bool includePlanStatus)
        {
            var data = new List<Icd9>();
            Icd9 icd9 = null;
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    var code = reader.GetString(reader.GetOrdinal("Icd9"));
                    var description = reader.GetString(reader.GetOrdinal("Description"));
                    var icd10Codes = reader.GetString(reader.GetOrdinal("Icd10Codes"));
                    //var insPlanCount = !reader.IsDBNull(reader.GetOrdinal("InsPlanCount")) ? reader.GetInt32(reader.GetOrdinal("InsPlanCount")) : null;
                    var insPlanCount = reader.GetInt32(reader.GetOrdinal("InsPlanCount"));
                    var enabledOnPlan = includePlanStatus ? reader.GetInt32(reader.GetOrdinal("EnabledOnPlan")) : 0;

                    icd9 = new Icd9()
                    {
                        code = code,
                        description = description,
                        icd10Codes = icd10Codes,
                        insPlanCount = insPlanCount,
                        enabledOnPlan = enabledOnPlan == 1 ? true : false
                    };
                    data.Add(icd9);
                }

                reader.Close();
            }
            return data;
        }

        private Icd9 MapReaderToIcd9(SqlDataReader reader)
        {
            Icd9 data = null;
            if (reader != null && !reader.IsClosed && reader.Read())
            {
                var code = reader.GetString(reader.GetOrdinal("Icd9"));
                var description = reader.GetString(reader.GetOrdinal("Description"));
                var icd10Codes = reader.GetString(reader.GetOrdinal("Icd10Codes"));
                var insPlanCount = reader.GetInt32(reader.GetOrdinal("InsPlanCount"));
                data = new Icd9()
                {
                    code = code,
                    description = description,
                    icd10Codes = icd10Codes,
                    insPlanCount = insPlanCount
                };

                reader.Close();
            }
            return data;
        }

        #endregion
    }
}