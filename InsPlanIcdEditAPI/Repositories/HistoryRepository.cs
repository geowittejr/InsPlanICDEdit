using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;

namespace InsPlanIcdEditApi.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        public HistoryRepository(string sqlDbConnectionString)
        {
            SqlDbConnectionString = sqlDbConnectionString;
        }
        private string SqlDbConnectionString = string.Empty;

        public List<HistoryTran> GetTrans(string entityId, string entityType, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalTrans)
        {
            //Get history transactions
            var data = new List<HistoryTran>();
            using (var connection = new SqlConnection(SqlDbConnectionString))
            {
                var sql = "sp_LMN_GetInsPlanDxHistory";
                var cmd = new SqlCommand(sql, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@EntityId", System.Data.SqlDbType.NVarChar).Value = entityId;
                cmd.Parameters.Add("@EntityType", System.Data.SqlDbType.NVarChar).Value = entityType;
                cmd.Parameters.Add("@StartIndex", System.Data.SqlDbType.Int).Value = startIndex;
                cmd.Parameters.Add("@EndIndex", System.Data.SqlDbType.Int).Value = endIndex;
                cmd.Parameters.Add("@SortColumn", System.Data.SqlDbType.NVarChar).Value = sortColumn;
                cmd.Parameters.Add("@SortDesc", System.Data.SqlDbType.Bit).Value = sortDesc ? 1 : 0;
                var totTransParm = cmd.Parameters.Add("@TotalTrans", System.Data.SqlDbType.Int);
                totTransParm.Direction = System.Data.ParameterDirection.Output;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);

                data = MapReaderToHistoryTranList(reader);

                int total = 0;
                int.TryParse(totTransParm.Value.ToString(), out total);
                totalTrans = total;
            }
            return data;
        }


        #region Mappers
        
        private List<HistoryTran> MapReaderToHistoryTranList(SqlDataReader reader)
        {
            var data = new List<HistoryTran>();
            HistoryTran tran = null;
            if (reader != null && !reader.IsClosed)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var icd9 = reader.GetString(reader.GetOrdinal("Icd9"));
                    var insPlanId = !reader.IsDBNull(reader.GetOrdinal("InsPlanId")) ? reader.GetString(reader.GetOrdinal("InsPlanId")) : null;
                    //var diseaseGroup = reader.GetString(reader.GetOrdinal("DiseaseGroup"));
                    var diseaseGroupId = !reader.IsDBNull(reader.GetOrdinal("DiseaseGroupId")) ? reader.GetInt32(reader.GetOrdinal("DiseaseGroupId")) : -1;
                    var actionType = reader.GetInt32(reader.GetOrdinal("ActionType"));
                    var username = reader.GetString(reader.GetOrdinal("Username"));
                    var actionDate = reader.GetDateTime(reader.GetOrdinal("ActionDate"));

                    tran = new HistoryTran()
                    {
                        id = id,
                        icd9 = icd9,
                        insPlanId = insPlanId,
                        //diseaseGroup = diseaseGroup,
                        diseaseGroupId = diseaseGroupId,
                        actionType = actionType,
                        username = username,
                        actionDate = actionDate
                    };
                    data.Add(tran);
                }

                reader.Close();
            }
            return data;
        }


        #endregion
    }
}