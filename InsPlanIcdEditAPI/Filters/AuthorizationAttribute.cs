using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using InsPlanIcdEditApi.Repositories;
using InsPlanIcdEditApi.Services;
using Microsoft.Practices.Unity;

namespace InsPlanIcdEditApi.Filters
{
    public class AuthorizationAttribute : System.Web.Http.AuthorizeAttribute
    {

        /*
            Found the basic idea behind this code at:
            https://github.com/rblaettler/BasicHttpAuthorization/blob/master/BasicHttpAuthorizeAttribute.cs    
         
            For the UserSvc property injection I used this:
            http://stackoverflow.com/questions/19130441/cant-inject-on-system-web-http-filters-actionfilterattribute-using-unity-bootst
         
        */
        public IUserService UserSvc { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //If the request is authorized continue. 
            if (RequestIsAuthorized(actionContext))
                return;

            //Otherwise, handle it as unauthorized.
            HandleUnauthorizedRequest(actionContext);
        }

        private bool RequestIsAuthorized(HttpActionContext actionContext) 
        {
            if (!HttpContext.Current.Request.Headers.AllKeys.Contains("Authorization")) return false;

            //Get the username from the header and save it to the request properties so we can look it up later
            string authHeader = HttpContext.Current.Request.Headers["Authorization"];     
            actionContext.Request.Properties.Add("CurrentUsername", authHeader);

            //Check to make sure this user is authorized
            var user = UserSvc.GetUser(authHeader);
            return user.isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }
    }
}