using System;
using System.Linq;
namespace PhoneBookData
{
    public interface IPhoneBookRepository<TEntity>
     where TEntity : class
    {
        void Delete(int id);
        TEntity GetById(int id);
        IQueryable<TEntity> GetEntities();
        TEntity Insert(TEntity entity);
        void Update(TEntity modifiedEntity);
    }
}
