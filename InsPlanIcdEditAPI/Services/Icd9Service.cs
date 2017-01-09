using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Repositories;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Services
{
    public class Icd9Service : IIcd9Service
    {
        public Icd9Service(IInsPlanIcdRepository insPlanRepo)
        {
            InsPlanIcdRepo = insPlanRepo;
        }
        private IInsPlanIcdRepository InsPlanIcdRepo = null;

        public List<Icd9> GetIcd9s(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalIcd9s)
        {
            return InsPlanIcdRepo.GetIcd9s(filterText, startIndex, endIndex, sortColumn, sortDesc, out totalIcd9s);
        }

        public Icd9 GetIcd9(string icd9)
        {
            return InsPlanIcdRepo.GetIcd9(icd9);
        }

        public List<InsPlan> GetIcd9InsPlans(string icd9, string filterText, int startIndex, int endIndex, string status, string sortColumn, bool sortDesc, out int totalPlans)
        {
            return InsPlanIcdRepo.GetIcd9InsPlans(icd9, filterText, startIndex, endIndex, status, sortColumn, sortDesc, out totalPlans);
        }

        public bool AddIcd9InsPlan(string icd9, string planId, string username)
        {
            return InsPlanIcdRepo.AddInsPlanIcd9(planId, icd9, username);
        }

        public bool RemoveIcd9InsPlan(string icd9, string planId, string username)
        {
            return InsPlanIcdRepo.RemoveInsPlanIcd9(planId, icd9, username);
        }

        public void PopulateIcd9CodesTable()
        {
            InsPlanIcdRepo.PopulateIcd9CodesTable();
        }
    }
}