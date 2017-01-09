using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Repositories;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Services
{
    public class InsPlanService : IInsPlanService
    {
        public InsPlanService(IInsPlanIcdRepository insPlanRepo)
        {
            InsPlanIcdRepo = insPlanRepo;
        }
        private IInsPlanIcdRepository InsPlanIcdRepo = null;
        
        public List<InsPlan> GetInsPlans(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalPlans)
        {
            return InsPlanIcdRepo.GetInsPlans(filterText, startIndex, endIndex, sortColumn, sortDesc, out totalPlans);
        }

        public InsPlan GetInsPlan(string planId)
        {
            return InsPlanIcdRepo.GetInsPlan(planId);
        }

        public List<Icd9> GetInsPlanIcd9s(string planId, string filterText, int startIndex, int endIndex, string status, string sortColumn, bool sortDesc, out int totalIcd9s)
        {
            totalIcd9s = 0;
            return InsPlanIcdRepo.GetInsPlanIcd9s(planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc, out totalIcd9s);
        }

        public bool AddInsPlanIcd9(string planId, string icd9, string username)
        {
            return InsPlanIcdRepo.AddInsPlanIcd9(planId, icd9, username);
        }

        public bool RemoveInsPlanIcd9(string planId, string icd9, string username)
        {
            return InsPlanIcdRepo.RemoveInsPlanIcd9(planId, icd9, username);
        }

        public void PopulateInsPlansTable()
        {
            InsPlanIcdRepo.PopulateInsPlansTable();
        }
    }
}