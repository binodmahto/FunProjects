using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Microsoft.Net.Http.Headers;

namespace CoreWebAPIDemo.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            if (!IsUserAuthorized(context))
                context.Result = new JsonResult(new { message = "Unauthorised" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        private bool IsUserAuthorized(ActionContext actionContext)
        {
            var accessToken = actionContext.HttpContext.Request.Headers[HeaderNames.Authorization][0].Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(accessToken))
            {
                var authService = (IJwtAuthManager)actionContext.HttpContext.RequestServices.GetService(typeof(IJwtAuthManager));
                var claimPrincipal = authService.DecodeJwtToken(accessToken).Item1;
                return claimPrincipal.Identity.IsAuthenticated;
            }
            return false;
        }
    }
}
