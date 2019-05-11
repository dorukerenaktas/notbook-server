using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NotBook.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;
        private readonly DbSet<T> _set;

        public BaseRepository(Context context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public void Insert(T entity)
        {
            _set.Add(entity);
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public void SaveAll(bool continueOnConflict = false)
        {
            _context.SaveChanges();
        }

        #region Properties

        public IQueryable<T> Table => _context.Set<T>();

        #endregion
    }
}