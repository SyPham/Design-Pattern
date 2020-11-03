using DMR_API._Repositories.Repositories;
using DMR_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EC_API.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IRepositoryFactory where TContext : DbContext, IDisposable
    {
        private IDbContextTransaction dbContextTransaction;
        public TContext Context { get; }

        private Dictionary<(Type type, string name), object> _repositories;
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task BeginTransactionAsync()
        {
            dbContextTransaction = await Context?.Database.BeginTransactionAsync();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {

                await dbContextTransaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task RollbackAsync()
        {
            await dbContextTransaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
           await Context.SaveChangesAsync();
        }

        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        internal object GetOrAddRepository(Type type, object repo)
        {
            _repositories ??= new Dictionary<(Type type, string Name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;
            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }
        public IECRepository<T> GetRepository<T>() where T : class
        {
            return (IECRepository<T>)GetOrAddRepository(typeof(T), new ECRepository<T>(Context));
        }
    }
}
