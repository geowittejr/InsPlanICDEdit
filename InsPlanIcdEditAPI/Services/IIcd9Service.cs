using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Services
{
    public interface IIcd9Service
    {
        List<Icd9> GetIcd9s(string filterText, int startIndex, int endIndex, string sortColumn, bool sortDesc, out int totalIcd9s);
        Icd9 GetIcd9(string icd9);
        List<InsPlan> GetIcd9InsPlans(string icd9, string filterText, int startIndex, int endIndex, string status, string sortColumn, bool sortDesc, out int totalPlans);
        bool AddIcd9InsPlan(string icd9, string planId, string username);
        bool RemoveIcd9InsPlan(string icd9, string planId, string username);
        void PopulateIcd9CodesTable();
    }
}