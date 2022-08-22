using Catalog.Models;
using Catalog.Repository;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Repository.Impl
{
    public class Repository<TId, TEntity, TContext> : IRepository<TId, TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;

        public Repository(TContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAll() => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> Get(Guid id) => await _context.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Save(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
