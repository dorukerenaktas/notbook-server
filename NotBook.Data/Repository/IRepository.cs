using System.Linq;

namespace NotBook.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        void Insert(T entity);

        void Delete(T entity);

        void SaveAll(bool continueOnConflict = false);
    }
}