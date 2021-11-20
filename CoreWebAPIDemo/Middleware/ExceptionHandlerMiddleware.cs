using CoreWebAPIDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoreWebAPIDemo.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly LoggerServiceResolver _serviceResolver;
        private readonly IConfiguration _configuration;

        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, IConfiguration configuraiton, LoggerServiceResolver serviceResolver)
        {
            _requestDelegate = requestDelegate;
            _serviceResolver = serviceResolver;
            _configuration = configuraiton;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            var logType = _configuration.GetValue<string>("UserLogger");
            var myLogger = _serviceResolver(logType);
            myLogger.Log(LogType.Error, ex);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(ex.Message);
        }
    }
}
