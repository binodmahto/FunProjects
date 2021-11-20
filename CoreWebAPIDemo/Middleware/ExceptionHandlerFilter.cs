using CoreWebAPIDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace CoreWebAPIDemo.Middleware
{
    public class ExceptionHandlerFilter : Attribute, IActionFilter
    {
        private readonly LoggerServiceResolver _serviceResolver;
        private readonly IConfiguration _configuration;

        public ExceptionHandlerFilter(IConfiguration configuraiton, LoggerServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
            _configuration = configuraiton;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var logType = _configuration.GetValue<string>("UserLogger");
            var myLogger = _serviceResolver(logType);
            if (context.Exception is HttpResponseException httpEexception)
            {
                context.Result = new ObjectResult(httpEexception.Value)
                {
                    StatusCode = httpEexception.Status,
                };
                context.ExceptionHandled = true;
            }
            if (context.Exception is Exception exception)
            {
                myLogger.Log(LogType.Error, exception);
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
                context.ExceptionHandled = true;
            }
        }
    }

    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}
