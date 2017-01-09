using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsPlanIcdEditApi.Models
{
    public class HistoryTran
    {
        public HistoryTran()
        {
        }
        public int id { get; set; }
        public string icd9 { get; set; }
        public string insPlanId { get; set; }
        public string diseaseGroup { get; set; }
        public int diseaseGroupId { get; set; }
        public int actionType { get; set; }
        public string username { get; set; }
        public DateTime actionDate { get; set; }
    }
}