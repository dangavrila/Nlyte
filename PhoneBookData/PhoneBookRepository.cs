using System;
using System.Data.Entity;
using System.Linq;

namespace PhoneBookData
{
    public class PhoneBookRepository<TEntity> : IPhoneBookRepository<TEntity>
        where TEntity : class
    {
        private PhoneBookContext _context;
        private DbSet<TEntity> _entitySet;

        public PhoneBookRepository(PhoneBookContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetEntities()
        {
            return _entitySet.AsQueryable<TEntity>();
        }

        public virtual TEntity GetById(int id)
        {
            return _entitySet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return _entitySet.Add(entity);
        }

        public virtual void Update(TEntity modifiedEntity)
        {
            _entitySet.Attach(modifiedEntity);
            _context.Entry(modifiedEntity).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var entityToDelete = _entitySet.Find(id);
            _entitySet.Remove(entityToDelete);
        }
    }
}
