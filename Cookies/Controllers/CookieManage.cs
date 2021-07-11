using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookieManage : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCookie([FromBody] string cookieKey)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Append(cookieKey, "MyCookieValue", cookieOptions);
            return Ok();
        }

        [HttpGet]
        [Route("Read")]
        public string ReadCookie([FromBody] string cookieKey)
        {
            return Request.Cookies[cookieKey];
        }

        [HttpGet]
        [Route("ReadAll")]
        public Dictionary<string,string> ReadAllCookie()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Request.Cookies.ToList().ForEach(c => dictionary.Add(c.Key, c.Value));
            return dictionary;
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult DeleteCookie([FromBody] string cookiesKey)
        {
            if(Request.Cookies[cookiesKey] is not null)
            {
                Response.Cookies.Delete(cookiesKey);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
