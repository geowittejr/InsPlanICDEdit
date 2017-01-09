using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsPlanIcdEditAPI.Models
{
    public class InsPlan
    {
        public InsPlan()
        {
            icd9Count = 0;
            enabledOnIcd9 = false;
        }
        public InsPlan(string planId, string insCoId, string insCoDesc, string insCoIdAlt, string insCoDescAlt, int icd9Count, bool enabledOnIcd9)
        {
            this.planId = planId;
            this.insCoId = insCoId;
            this.insCoDesc = insCoDesc;
            this.insCoIdAlt = insCoIdAlt;
            this.insCoDescAlt = insCoDescAlt;
            this.icd9Count = icd9Count;
            this.enabledOnIcd9 = enabledOnIcd9;
        }
        public string planId { get; set; }
        public string insCoId { get; set; }
        public string insCoDesc { get; set; }
        public string insCoIdAlt { get; set; }
        public string insCoDescAlt { get; set; }
        public int icd9Count { get; set; }
        public bool enabledOnIcd9 { get; set; }
    }
}