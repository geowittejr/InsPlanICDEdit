using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsPlanIcdEditAPI.Models
{
    public class Icd9
    {
        public Icd9()
        {
            insPlanCount = 0;
            enabledOnPlan = false;
        }
        public Icd9(string code, string description, string icd10Codes, int insPlanCount, bool enabledOnPlan)
        {
            this.code = code;
            this.description = description;
            this.icd10Codes = icd10Codes;
            this.insPlanCount = insPlanCount;
            this.enabledOnPlan = enabledOnPlan;
        }
        public string code { get; set; }
        public string description { get; set; }
        public string icd10Codes { get; set; }
        public int insPlanCount { get; set; }
        public bool enabledOnPlan { get; set; }
    }
}