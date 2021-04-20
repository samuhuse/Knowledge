using HostedServices.Data;
using HostedServices.Model;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HostedServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    { 
        [HttpPost]
        public async Task<bool> Send([FromServices] Channel<User> channel, string name)
        {
            await channel.Writer.WriteAsync(new User() { Name = name});
            return true;
        }

        [HttpPost]
        [Route("WithoutChannel")]
        public async Task<bool> SendWithoutChannel([FromServices] Database database, [FromServices] IHttpClientFactory httpClientFactory, string name)
        {
            User user = new User() { Name = name };

            var client = httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://docs.microsoft.com/en-us/dotnet/core/");
            user.Message = response;

            database.Users.Add(user);
            return 0 < await database.SaveChangesAsync();
        }

        [HttpGet]
        public List<User> Get([FromServices] Database database)
        {
            return database.Users.ToList();
        }
    }
}
