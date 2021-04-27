using Microsoft.EntityFrameworkCore;
using TicketManagerAPI.Database.Models;

namespace TicketManagerAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Ticket> Ticket { get; set; }
    }
}