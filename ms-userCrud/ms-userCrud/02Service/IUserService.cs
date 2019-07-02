using ms_userCrud._01Api;
using ms_userCrud._01Api.Model;
using ms_userCrud._03Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_userCrud._02Service
{
    public interface IUserService
    {
        Token Authentication(User user);
        int InsertUser(User user);
        User UpdateUser(int id, User user);
        int DeleteUser(int id);
        IList<UserDTO> List();
        User GetById(int id);
    }
}
