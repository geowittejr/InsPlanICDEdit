using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using InsPlanIcdEditApi.IoC;
using InsPlanIcdEditApi.Repositories;
using InsPlanIcdEditApi.Services;
using InsPlanIcdEditApi.Controllers;
using Microsoft.Practices.Unity;
using System.Configuration;
using InsPlanIcdEditApi.Handlers;
using InsPlanIcdEditApi.Filters;
using System.Web.Http.Filters;
using System.Threading;

namespace InsPlanIcdEditApi
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Create global logger name
            var appName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.LoggerName = appName + "Logger";
            
            //Create logging ability for this class
            LoggableClass = new LoggableClassInstance(this.LoggerName, "Global.asax.cs");

            //Configure IoC
            GlobalContainer = ConfigureIoC();
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolverUnity(GlobalContainer); //allow IoC container to resolve all dependencies

            //Register a filter provider that works with Unity.
            //This step is necessary to get the property injection working in our AuthorizationAttribute class for the UserService.
            //See this: http://stackoverflow.com/questions/19130441/cant-inject-on-system-web-http-filters-actionfilterattribute-using-unity-bootst
            RegisterFilterProviders(GlobalContainer);

            //Get handlers and filters
            var corsHandler = GlobalContainer.Resolve<CorsMessageHandler>(); //handles CORS requests
            var authAttribute = GlobalContainer.Resolve<AuthorizationAttribute>();
            var logAttribute = GlobalContainer.Resolve<LogAttribute>(); //enables logging controller method exceptions

            WebApiConfig.Register(GlobalConfiguration.Configuration, corsHandler, authAttribute, logAttribute);

            //Populate the InsPlans and Icd9Codes tables on a specific time interval
            populateIcd9CodesTableCallback = new TimerCallback(PopulateIcd9CodesTable);
            populateIcd9CodesTableTimer = new Timer(populateIcd9CodesTableCallback, null, 1000, 300000); //start in one second and then run every five minutes

            populateInsPlansTableCallback = new TimerCallback(PopulateInsPlansTable);
            populateInsPlansTableTimer = new Timer(populateInsPlansTableCallback, null, 1000, 300000); //start in one second and then run every five minutes
        }
        protected IUnityContainer GlobalContainer = null;
        protected string LoggerName = string.Empty; //Global logger name
        protected LoggableClassInstance LoggableClass = null; //Allows logging in this class
        protected DateTime populateIcd9CodesTableLastRunDate = DateTime.MinValue;
        protected Timer populateIcd9CodesTableTimer = null;
        protected TimerCallback populateIcd9CodesTableCallback = null;
        protected DateTime populateInsPlansTableLastRunDate = DateTime.MinValue;
        protected Timer populateInsPlansTableTimer = null;
        protected TimerCallback populateInsPlansTableCallback = null;

        //This method populates the ICD9 Codes table on a scheduled interval
        protected void PopulateIcd9CodesTable(Object stateInfo)
        {
            var interval = Convert.ToInt32(ConfigurationManager.AppSettings["PopulateIcd9CodesTableIntervalMinutes"] ?? "60");        
            var timeDiff = DateTime.Now - populateIcd9CodesTableLastRunDate;
            if (timeDiff.TotalMinutes < interval)
                return;

            var svc = GlobalContainer.Resolve<IIcd9Service>();
            svc.PopulateIcd9CodesTable();
            populateIcd9CodesTableLastRunDate = DateTime.Now;
        }

        //This method populates the InsPlans table on a scheduled interval
        protected void PopulateInsPlansTable(Object stateInfo)
        {
            var interval = Convert.ToInt32(ConfigurationManager.AppSettings["PopulateInsPlansTableIntervalMinutes"] ?? "60"); 
            var timeDiff = DateTime.Now - populateInsPlansTableLastRunDate;
            if (timeDiff.TotalMinutes < interval)
                return;

            var svc = GlobalContainer.Resolve<IInsPlanService>();
            svc.PopulateInsPlansTable();
            populateInsPlansTableLastRunDate = DateTime.Now;
        }

        // For the UserSvc property injection I used this:
        // http://stackoverflow.com/questions/19130441/cant-inject-on-system-web-http-filters-actionfilterattribute-using-unity-bootst
        private static void RegisterFilterProviders(IUnityContainer container)
        {
            var providers = GlobalConfiguration.Configuration.Services.GetFilterProviders().ToList();

            GlobalConfiguration.Configuration.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new UnityActionFilterProvider(container));

            var defaultprovider = providers.First(p => p is ActionDescriptorFilterProvider);

            GlobalConfiguration.Configuration.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultprovider);
        }

        protected IUnityContainer ConfigureIoC()
        {
            var container = new UnityContainer();

            var key = ConfigurationManager.ConnectionStrings["SqlDbConnectionString"];
            var sqlDbConnectionString = key != null ? key.ConnectionString : string.Empty;
            
            //Register repos
            container.RegisterType<IInsPlanIcdRepository, InsPlanIcdRepository>(new InjectionConstructor(sqlDbConnectionString));
            container.RegisterType<IHistoryRepository, HistoryRepository>(new InjectionConstructor(sqlDbConnectionString));
            container.RegisterType<IUserRepository, UserRepository>(new InjectionConstructor(sqlDbConnectionString));

            //Register services
            container.RegisterType<IInsPlanService, InsPlanService>();
            container.RegisterType<IIcd9Service, Icd9Service>();
            container.RegisterType<IHistoryService, HistoryService>();
            container.RegisterType<IUserService, UserService>();

            //Register controllers
            container.RegisterType<InsPlanController>(new InjectionConstructor(container.Resolve<IInsPlanService>()));
            container.RegisterType<Icd9Controller>(new InjectionConstructor(container.Resolve<IIcd9Service>()));
            container.RegisterType<HistoryTranController>(new InjectionConstructor(container.Resolve<IHistoryService>()));
            container.RegisterType<UserController>(new InjectionConstructor(container.Resolve<IUserService>()));

            //Register handlers
            container.RegisterType<CorsMessageHandler>();

            //Register filters
            container.RegisterType<AuthorizationAttribute>(new InjectionProperty("UserSvc"));
            container.RegisterType<LogAttribute>(new InjectionConstructor(this.LoggerName));

            return container;
        }

        //Catch-all for errors we didn't think of
        protected void Application_Error()
        {
            var exc = Server.GetLastError();
            if (LoggableClass.ErrorLogIsEnabled) LoggableClass.ErrorLog("Exception", "Application_Error", exc);
        }  
    }
}