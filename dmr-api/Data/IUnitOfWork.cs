using DMR_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EC_API.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        Task<bool> CommitAsync();
        Task RollbackAsync();
        IECRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
