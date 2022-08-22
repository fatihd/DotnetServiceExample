using Catalog.Service;

namespace Catalog.Repository
{
    public interface IRepository<TId, TEntity>
    {
        Task<TEntity?> Get(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Save(TEntity entity);
        
        Task SaveChanges();
   }
}
