using ms_userCrud.Api.Model;
using ms_userCrud.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_userCrud.Service.Mapper
{
    public static class UserMapper
    {
        public static UserDTO ConvertUserToDTO(User user)
        {
            return new UserDTO()
            {
                Document = user.Document,
                Email = user.Email,
                Name = user.Name,
                Id = user.Id,
                Password = user.Password,
                Username = user.Username
            };
        }

        public static User ConvertDTOToUser(UserDTO user)
        {
            return new User()
            {
                Document = user.Document,
                Email = user.Email,
                Name = user.Name,
                Id = user.Id,
                Password = user.Password,
                Username = user.Username
            };
        }
    }
}
