using Microsoft.EntityFrameworkCore;
using PraktykiServer.Models;

namespace PraktykiServer.Data
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
    }
}
