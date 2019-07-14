using ms_userCrud.Api;
using ms_userCrud.Api.Model;
using ms_userCrud.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_userCrud.Service
{
    public interface IUserService
    {
        Token Authentication(User user);
        User InsertUser(User user);
        User UpdateUser(int id, User user);
        int DeleteUser(int id);
        IList<UserDTO> List();
        User GetById(int id);
    }
}
