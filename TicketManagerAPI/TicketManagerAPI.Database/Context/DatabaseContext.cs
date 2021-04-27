using Microsoft.EntityFrameworkCore;
using TicketManagerAPI.Database.Models;

namespace TicketManagerAPI.Database.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=WINDOWS-JN3O5DG;Database=TicketManagerBD;Trusted_Connection=True;");
        }
    }
}