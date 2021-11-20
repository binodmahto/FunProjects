using CoreWebAPIDemo.Middleware;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreWebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [ExceptionHandlerFilter]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(int value)
        {
            if (value == 1)
                return Ok("I'm Happy");
            else
                throw new Exception("I didn't like your number");
        }
    }
}
