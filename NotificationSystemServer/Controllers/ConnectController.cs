using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktykiServer.Data;
using PraktykiServer.Models;

namespace PraktykiServer.Controllers
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
        public IActionResult Connect([FromBody] GuidData guidData)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.guid == guidData.Guid);
            if (existingClient != null)
            {
                return Ok();
            }

            var newClient = new Client { guid = guidData.Guid };
            _context.Clients.Add(newClient);
            _context.SaveChanges();

            return Ok();
        }

        public class GuidData
        {
            public Guid Guid { get; set; }
        }
    }

    
}
