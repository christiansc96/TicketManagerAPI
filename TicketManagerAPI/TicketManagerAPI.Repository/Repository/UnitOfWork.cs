using System;
using System.Threading.Tasks;
using TicketManagerAPI.Database.Context;

namespace TicketManagerAPI.Repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseContext Context { get; }
        Task Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public DatabaseContext Context { get; }
        public UnitOfWork(DatabaseContext context) => Context = context;

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose() => Context.Dispose();
    }
}