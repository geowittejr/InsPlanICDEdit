using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InsPlanIcdEditApi.Services;
using InsPlanIcdEditAPI.Models;
using InsPlanIcdEditApi.Filters;

namespace InsPlanIcdEditApi.Controllers
{
    //[RoutePrefix("icd9s")]
    public class HistoryTranController : ApiController
    {
        public HistoryTranController(IHistoryService historySvc)
        {
            HistorySvc = historySvc;
        }
        private IHistoryService HistorySvc = null;

        // GET - get a list of history transactions
        [HttpGet]
        [Authorization]
        public HttpResponseMessage GetTrans(string id, string type, int start, int end, string sortCol, int sortDesc)
        {
            if (id != null && type != null && type.ToLower() == "icd9")
                id = id.Replace("-", "."); //Can't pass dots in urls, so we converted them to dashes. Convert back.
            
            var sortDescending = sortDesc == 1 ? true : false; //convert to bool
            var totalTrans = 0; //output parm

            //Get the data
            var trans = HistorySvc.GetTrans(id, type, start, end, sortCol, sortDescending, out totalTrans);

            var endIndex = end >= totalTrans ? totalTrans - 1 : end; //return the actual ending row index

            var returnObj = new Dictionary<string, object>();
            returnObj.Add("trans", trans);
            returnObj.Add("totalTrans", totalTrans);
            returnObj.Add("startIndex", start);
            returnObj.Add("endIndex", endIndex);
            
            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }
    }
}