using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IdentityServer4APIDemo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// The preceding Error action sends an RFC 7807-compliant payload to the client.
        /// </summary>
        /// <returns></returns>
        [Route("/error")]
        protected IActionResult Error() => Problem("Oops there is a problem. Excuse me and smile!");

        [Route("/error-local-development")]
        protected IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("Ugggh! This is not for Production, go away.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}
