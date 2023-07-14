namespace NotificationSystemServer.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NotificationSystemServer.Data;
    using NotificationSystemServer.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly ClientDbContext context;

        public ConnectController(ClientDbContext context) => this.context = context;

        [HttpPost]
        public IActionResult Connect([FromBody] TokenData tokenData)
        {
            var existingClient = this.context.Clients.FirstOrDefault(c => c.FCMToken == tokenData.FCMToken);
            if (existingClient != null)
            {
                return this.Ok();
            }

            var newClient = new Client { FCMToken = tokenData.FCMToken };
            this.context.Clients.Add(newClient);
            this.context.SaveChanges();

            return this.Ok();
        }

        public class TokenData
        {
            public string? FCMToken { get; set; }
        }
    }
}
