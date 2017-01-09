using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace InsPlanIcdEditApi.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public LogAttribute(string loggerName)
        {
            LoggableClass = new LoggableClassInstance(loggerName, "LogAttribute");
        }
        private LoggableClassInstance LoggableClass = null;

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //Log controller method exceptions

            if (actionExecutedContext.Exception != null && LoggableClass.ErrorLogIsEnabled)
            {
                var className = actionExecutedContext.ActionContext.ControllerContext.Controller.ToString();
                var methodName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var methodArgs = actionExecutedContext.ActionContext.ActionArguments;
                var exc = actionExecutedContext.Exception;

                LoggableClass.ClassName = className;

                if (LoggableClass.ErrorLogIsEnabled) LoggableClass.ErrorLog("Exception", methodName, exc, "MethodArgs", methodArgs);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}