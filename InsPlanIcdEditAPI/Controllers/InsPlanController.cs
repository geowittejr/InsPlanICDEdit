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
    //Commented out attribute routing version used with Web API 2.0 and .NET 4.5
    //[RoutePrefix("plans")]
    public class InsPlanController : ApiController
    {
        public InsPlanController(IInsPlanService insPlanSvc)
        {
            InsPlanSvc = insPlanSvc;
        }
        private IInsPlanService InsPlanSvc = null;
        public IUserService UserSvc = null;
        
        // GET - get a list of insurance plans            
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetInsPlans(string filter, int start, int end, string sortCol, int sortDesc)
        {
            var sortDescending = sortDesc == 1 ? true : false; //convert to bool
            var totalPlans = 0; //output parm
            
            //Get the data
            var plans = InsPlanSvc.GetInsPlans(filter, start, end, sortCol, sortDescending, out totalPlans);

            var endIndex = end >= totalPlans ? totalPlans - 1 : end; //return the actual ending row index
            
            var returnObj = new Dictionary<string, object>();
            returnObj.Add("plans", plans);
            returnObj.Add("totalPlans", totalPlans);
            returnObj.Add("startIndex", start);
            returnObj.Add("endIndex", endIndex);

            //System.Threading.Thread.Sleep(1000);
            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }
        
        // GET - get details of insurance plan
        //Commented out attribute routing version used with Web API 2.0 and .NET 4.5
        //[HttpGet("{planId}")]
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetInsPlan(string planId)
        {
            var data = InsPlanSvc.GetInsPlan(planId);
            var returnObj = new Dictionary<string, object>();
            returnObj.Add("plan", data);
            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }

        // GET - get list of ICD9s for insurance plan
        //Commented out attribute routing version used with Web API 2.0 and .NET 4.5
        //[HttpGet("{planId}/icd9s")]
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetInsPlanIcd9s(string planId, string filter, int start, int end, string status, string sortCol, int sortDesc)
        {
            var sortDescending = sortDesc == 1 ? true : false; //convert to bool
            var totalIcd9s = 0;  //output parm

            //Get the data
            var icd9s = InsPlanSvc.GetInsPlanIcd9s(planId, filter, start, end, status, sortCol, sortDescending, out totalIcd9s);

            var endIndex = end >= totalIcd9s ? totalIcd9s - 1 : end; //return the actual ending row index

            var returnObj = new Dictionary<string, object>();
            returnObj.Add("icd9s", icd9s);
            returnObj.Add("totalIcd9s", totalIcd9s);
            returnObj.Add("startIndex", start);
            returnObj.Add("endIndex", endIndex);

            //System.Threading.Thread.Sleep(1000);
            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }

        // PUT - add an ICD9 code to the insurance plan
        //Commented out attribute routing version used with Web API 2.0 and .NET 4.5
        //[HttpPut("{planId}/icd9s/{icd9}")]
        [HttpPut]
        [Authorization]
        public HttpResponseMessage PutInsPlanIcd9(string planId, string icd9)
        {            
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            var success = InsPlanSvc.AddInsPlanIcd9(planId, icd9, CurrentUsername);
            //System.Threading.Thread.Sleep(500);
            return Request.CreateResponse<bool>(HttpStatusCode.OK, success);                
        }

        // DELETE - remove an ICD9 code from the insurance plan
        //Commented out attribute routing version used with Web API 2.0 and .NET 4.5
        //[HttpDelete("{planId}/icd9s/{icd9}")]
        [HttpDelete]  
        [Authorization]
        public HttpResponseMessage DeleteInsPlanIcd9(string planId, string icd9)
        {
            icd9 = icd9.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            var success = InsPlanSvc.RemoveInsPlanIcd9(planId, icd9, CurrentUsername);
            //System.Threading.Thread.Sleep(500);

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