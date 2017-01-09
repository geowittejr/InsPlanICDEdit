using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using InsPlanIcdEditApi.Handlers;
using InsPlanIcdEditApi.Filters;

namespace InsPlanIcdEditApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, CorsMessageHandler corsHandler, AuthorizationAttribute authAttribute, LogAttribute logAttribute)
        {
            //Add handlers

            //Requests are processed by message handlers in the same order that the message handlers are added here. So...
            //Add CorsHandler first because preflight requests won't hit other handlers if the CORS headers are not added.
            config.MessageHandlers.Add(corsHandler); //Do not add other handlers before this one!!

            //Add filters

            //uncomment next line to globally require authorization attribute, instead of by controller method
            //config.Filters.Add(authAttribute); //enables authorization of requests            
            config.Filters.Add(logAttribute); //enables logging of controller method exceptions


            //Configure attribute routing for Web API 2.0 (requires .NET 4.5)
            //Uncomment the line below to enable attribute routing if we ever upgrade to .NET 4.5
            //config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "GetInsPlansRoute",
                routeTemplate: "plans/",
                defaults: new
                {
                    controller = "InsPlan",
                    action = "GetInsPlans"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetInsPlanRoute",
                routeTemplate: "plans/{planId}",
                defaults: new
                {
                    controller = "InsPlan",
                    action = "GetInsPlan"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetInsPlanIcd9sRoute",
                routeTemplate: "plans/{planId}/icd9s",
                defaults: new
                {
                    controller = "InsPlan",
                    action = "GetInsPlanIcd9s"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PutInsPlanIcd9Route",
                routeTemplate: "plans/{planId}/add/{icd9}",                
                defaults: new
                {
                    controller = "InsPlan",
                    action = "PutInsPlanIcd9"                    
                }
            );

            config.Routes.MapHttpRoute(
                name: "DeleteInsPlanIcd9Route",
                routeTemplate: "plans/{planId}/remove/{icd9}",
                defaults: new
                {
                    controller = "InsPlan",
                    action = "DeleteInsPlanIcd9"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetIcd9sRoute",
                routeTemplate: "icd9s/",
                defaults: new
                {
                    controller = "Icd9",
                    action = "GetIcd9s"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetIcd9Route",
                routeTemplate: "icd9s/{icd9}",
                defaults: new
                {
                    controller = "Icd9",
                    action = "GetIcd9"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetIcd9InsPlansRoute",
                routeTemplate: "icd9s/{icd9}/plans",
                defaults: new
                {
                    controller = "Icd9",
                    action = "GetIcd9InsPlans"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PutIcd9InsPlanRoute",
                routeTemplate: "icd9s/{icd9}/add/{planId}",
                defaults: new
                {
                    controller = "Icd9",
                    action = "PutIcd9InsPlan"
                }
            );

            config.Routes.MapHttpRoute(
                name: "DeleteIcd9InsPlanRoute",
                routeTemplate: "icd9s/{icd9}/remove/{planId}",
                defaults: new
                {
                    controller = "Icd9",
                    action = "DeleteIcd9InsPlan"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetHistoryTransRoute",
                routeTemplate: "history/",
                defaults: new
                {
                    controller = "HistoryTran",
                    action = "GetTrans"
                }
            );

            config.Routes.MapHttpRoute(
                name: "GetUserRoute",
                routeTemplate: "users/{username}",
                defaults: new
                {
                    controller = "User",
                    action = "GetUser"
                }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiRoute",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}