using ActionFilters.DTOs;
using ActionFilters.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ActionFilters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        [HttpGet("FavoriteNumber")]
        [LogActionFilter] // Log every step of the action
        public IActionResult GetFavoriteNumber()
        {
            return Ok(new Random().Next(1, 100));
        }

        [HttpPost("ModelStateAutoValidation")]
        [ValidateRequestFilter]
        public IActionResult ModelStateAutoValidation([FromBody] ValidableDto request)
        {
            return Ok();
        }

        [HttpGet("Random")]
        [ResponseCache(Duration = 5)] // Cache the result for 5 second
        public IActionResult GetRandomNumber()
        {
            return Ok(new Random().Next(1, 100));
        }

        [HttpGet("Exception")]
        [LogExceptionFilter] // log the exception
        public IActionResult GetException()
        {
            throw new Exception("Custom Exception Message");
        }
    }
}
