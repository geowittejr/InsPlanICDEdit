using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Services
{
    public interface IInsPlanService
    {
        InsPlan GetInsPlan(string planId);
        List<InsPlan> GetInsPlans(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalPlans);
        List<Icd9> GetInsPlanIcd9s(string planId, string filterText, int startIndex, int endIndex, string status, string sortColumn, bool sortDesc, out int totalIcd9s);
        bool AddInsPlanIcd9(string planId, string icd9, string username);
        bool RemoveInsPlanIcd9(string planId, string icd9, string username);
        void PopulateInsPlansTable();
    }
}