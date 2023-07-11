using Microsoft.EntityFrameworkCore;
using NotificationSystemServer.Models;

namespace NotificationSystemServer.Data
{
    public class ClientDbContext : DbContext
    {


        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
        {
 
        }


        public DbSet<Client> Clients { get; set; }
    }
}
