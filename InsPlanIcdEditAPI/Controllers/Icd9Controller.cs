using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InsPlanIcdEditApi.Filters;
using InsPlanIcdEditApi.Services;
using InsPlanIcdEditAPI.Models;

namespace InsPlanIcdEditApi.Controllers
{
    //[RoutePrefix("icd9s")]
    public class Icd9Controller : ApiController
    {
        public Icd9Controller(IIcd9Service icd9Svc)
        {
            Icd9Svc = icd9Svc;
        }
        private IIcd9Service Icd9Svc = null;

        // GET - get a list of icd9s
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetIcd9s(string filter, int start, int end, string sortCol, int sortDesc)
        {
            var sortDescending = sortDesc == 1 ? true : false; //convert to bool
            var totalIcd9s = 0; //output parm

            //Get the data
            var icd9s = Icd9Svc.GetIcd9s(filter, start, end, sortCol, sortDescending, out totalIcd9s);

            var endIndex = end >= totalIcd9s ? totalIcd9s - 1 : end; //return the actual ending row index

            var returnObj = new Dictionary<string, object>();
            returnObj.Add("icd9s", icd9s);
            returnObj.Add("totalIcd9s", totalIcd9s);
            returnObj.Add("startIndex", start);
            returnObj.Add("endIndex", endIndex);

            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }

        // GET - get details of icd9
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetIcd9(string icd9)
        {
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            var data = Icd9Svc.GetIcd9(icd9);            
            var returnObj = new Dictionary<string, object>();
            returnObj.Add("icd9", data);            
            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }     

        // GET - get list of ICD9s for insurance plan
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetIcd9InsPlans(string icd9, string filter, int start, int end, string status, string sortCol, int sortDesc)
        {
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.

            var sortDescending = sortDesc == 1 ? true : false; //convert to bool
            var totalPlans = 0;  //output parm

            //Get the data
            var plans = Icd9Svc.GetIcd9InsPlans(icd9, filter, start, end, status, sortCol, sortDescending, out totalPlans);

            var endIndex = end >= totalPlans ? totalPlans - 1 : end; //return the actual ending row index

            var returnObj = new Dictionary<string, object>();
            returnObj.Add("plans", plans);
            returnObj.Add("totalPlans", totalPlans);
            returnObj.Add("startIndex", start);
            returnObj.Add("endIndex", endIndex);

            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }

        [HttpPut]
        [Authorization]
        public HttpResponseMessage PutIcd9InsPlan(string icd9, string planId)
        {
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            var success = Icd9Svc.AddIcd9InsPlan(icd9, planId, CurrentUsername);            
            return Request.CreateResponse<bool>(HttpStatusCode.OK, success);                
        }

        [HttpDelete]  
        [Authorization]
        public HttpResponseMessage DeleteIcd9InsPlan(string icd9, string planId)
        {
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            var success = Icd9Svc.RemoveIcd9InsPlan(icd9, planId, CurrentUsername);            
            return Request.CreateResponse<bool>(HttpStatusCode.OK, success);
        }

        //Property to get the current username
        private string CurrentUsername
        {
            get
            {
                //Get username from request properties
                object usernameProp = null;
                Request.Properties.TryGetValue("CurrentUsername", out usernameProp);
                return usernameProp != null ? usernameProp.ToString() : "Anonymous";
            }
        }
    }
}