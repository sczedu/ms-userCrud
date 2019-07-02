using ms_userCrud._03Data.Entity;
using System.Collections.Generic;

namespace ms_userCrud._03Data
{
    public interface IUserContext<T> where T : UserDTO
    {
        int Insert(T obj);

        int Update(T obj);

        int Remove(int id);

        T Select(int id);

        IList<T> SelectAll();
    }
}


