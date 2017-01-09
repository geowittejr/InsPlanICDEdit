using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;

namespace InsPlanIcdEditApi.Services
{
    public interface IHistoryService
    {
        List<HistoryTran> GetTrans(string entityId, string entityType, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalTrans);
    }
}