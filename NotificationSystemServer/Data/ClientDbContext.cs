namespace NotificationSystemServer.Data
{
    using Microsoft.EntityFrameworkCore;
    using NotificationSystemServer.Models;

    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
    }
}
