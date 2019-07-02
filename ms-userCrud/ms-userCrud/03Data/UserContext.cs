using ms_userCrud._03Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace ms_userCrud._03Data
{
    public class UserContext<T> : IUserContext<T> where T : UserDTO
    {
        private DBContext _dbContext;

        public UserContext(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Insert(T obj)
        {
            _dbContext.Set<T>().Add(obj);
            return _dbContext.SaveChanges();
        }

        public int Update(T obj)
        {
            _dbContext.Entry(obj).State
                = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            _dbContext.Set<T>().Remove(Select(id));
            return _dbContext.SaveChanges();
        }

        public IList<T> Select()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Select(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public int Remove(int id)
        {
            var entity = Select(id);
            if(entity==null)
                return 0;
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public IList<T> SelectAll()
        {
            return _dbContext.Set<T>().ToList();
        }
    }
}
