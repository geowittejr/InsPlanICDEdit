using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;
using InsPlanIcdEditApi.Repositories;

namespace InsPlanIcdEditApi.Services
{
    public class HistoryService : IHistoryService
    {
        public HistoryService(IHistoryRepository historyRepo)
        {
            HistoryRepo = historyRepo;
        }
        private IHistoryRepository HistoryRepo = null;

        public List<HistoryTran> GetTrans(string entityId, string entityType, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalTrans)
        {
            return HistoryRepo.GetTrans(entityId, entityType, startIndex, endIndex, sortColumn, sortDesc, out totalTrans);
        }
    }
}