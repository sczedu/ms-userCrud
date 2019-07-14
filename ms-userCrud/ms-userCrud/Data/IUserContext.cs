using ms_userCrud.Data.Entity;
using System.Collections.Generic;

namespace ms_userCrud.Data
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


