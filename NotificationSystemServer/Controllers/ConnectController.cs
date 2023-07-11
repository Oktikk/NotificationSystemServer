using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationSystemServer.Data;
using NotificationSystemServer.Models;

namespace NotificationSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly ClientDbContext _context;

        public ConnectController(ClientDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Client>> Get() =>
            await _context.Clients.ToListAsync();

        [HttpPost]
        public IActionResult Connect([FromBody] TokenData TokenData)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.FCMToken == TokenData.FCMToken);
            if (existingClient != null)
            {
                return Ok();
            }

            var newClient = new Client { FCMToken = TokenData.FCMToken };
            _context.Clients.Add(newClient);
            _context.SaveChanges();

            return Ok();
        }

        public class TokenData
        {
            public String FCMToken { get; set; }
        }
    }

    
}
