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
    public class UserController : ApiController
    {
        public UserController(IUserService userSvc)
        {
            UserSvc = userSvc;
        }
        private IUserService UserSvc = null;

        [HttpGet]
        public HttpResponseMessage GetUser(string username)
        {
            //Get the data
            var usr = UserSvc.GetUser(username);

            var returnObj = new Dictionary<string, object>();
            returnObj.Add("user", usr);

            return Request.CreateResponse<Dictionary<string, object>>(HttpStatusCode.OK, returnObj);
        }
    }
}