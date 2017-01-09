using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Repositories
{
    public interface IInsPlanIcdRepository
    {
        InsPlan GetInsPlan(string planId);
        List<InsPlan> GetInsPlans(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalPlans);
        List<Icd9> GetInsPlanIcd9s(string planId, string filterText, int startIndex, int endIndex, string status, string sortColumn, bool sortDesc, out int totalIcd9s);
        List<Icd9> GetIcd9s(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalIcd9s);
        Icd9 GetIcd9(string icd9);
        List<InsPlan> GetIcd9InsPlans(string icd9, string filterText, int startIndex, int endIndex, string status, string sortColumns, bool sortDesc, out int totalPlans);
        bool AddInsPlanIcd9(string planId, string icd9, string username);
        bool RemoveInsPlanIcd9(string planId, string icd9, string username);
        void PopulateIcd9CodesTable();
        void PopulateInsPlansTable();        
    }
}