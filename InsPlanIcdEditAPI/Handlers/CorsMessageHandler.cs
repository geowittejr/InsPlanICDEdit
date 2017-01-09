using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Web;

namespace InsPlanIcdEditApi.Handlers
{
    /// <summary>
    /// This message handler enables CORS requests
    /// </summary>
    public class CorsMessageHandler : DelegatingHandler
    {
        // Found this code online at:
        // http://stackoverflow.com/questions/16269365/how-to-enable-both-cors-support-and-ntlm-authentication
        
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        public CorsMessageHandler()
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                    response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                    string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                    if (accessControlRequestMethod != null)
                    {
                        response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                    }

                    string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
                    if (!string.IsNullOrEmpty(requestedHeaders))
                    {
                        response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                    }

                    TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
                    tcs.SetResult(response);
                    
                    return tcs.Task;
                }
                else
                {
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = t.Result;
                        resp.Headers.Add(
                            AccessControlAllowOrigin,
                            request.Headers.GetValues(Origin).First());
                        
                        return resp;
                    });
                }
            }
            else
            {
                var response = base.SendAsync(request, cancellationToken);

                return response;
            }
        }
    }
}