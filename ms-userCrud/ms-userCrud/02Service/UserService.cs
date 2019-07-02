using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ms_userCrud._01Api.Model;
using ms_userCrud._01Api.Validator;
using ms_userCrud._03Data;
using ms_userCrud._03Data.Entity;
using ms_userCrud._05Helper;
using System.Collections.Generic;
using System.Linq;

namespace ms_userCrud._02Service
{
    public class UserService : IUserService
    {
        private readonly IAuthHelper _authenticationService;
        private readonly UserAuthValidator _userAuthValidator;
        private readonly UserValidator _userValidator;
        private readonly IHelper _helper;
        private readonly IUserContext<UserDTO> _userContext;

        public UserService(IAuthHelper authenticationService, UserAuthValidator userAuthValidator, IHelper helper, IUserContext<UserDTO> userContext, UserValidator userValidator)
        {
            _authenticationService = authenticationService;
            _userAuthValidator = userAuthValidator;
            _helper = helper;
            _userContext = userContext;
            _userValidator = userValidator;
        }


        public Token Authentication(User user)
        {
            var resultValidator = _userAuthValidator.Validate(user);
            if (!resultValidator.IsValid)
                _helper.ValidatorHandler(resultValidator);
            var hashPassword = _authenticationService.GetHash(user.Password);
            var userDb = _userContext.SelectAll().FirstOrDefault(f => f.Username == user.Username && f.Password == hashPassword);
            if (userDb == null)
                return null;
            return  _authenticationService.GenerateToken(user);
        }

        public int InsertUser(User user)
        {
            var resultValidator = new UserValidator().Validate(user);
            if (!resultValidator.IsValid)
                _helper.ValidatorHandler(resultValidator);
            user.Password = _authenticationService.GetHash(user.Password);
            var userDTO = ConvertUserToDTO(user);
            return _userContext.Insert(userDTO);
        }

        public User UpdateUser(int id, User user)
        {
            var oldUser = _userContext.Select(id);
            if (oldUser == null)
                return null;
            var resultValidator = new UserValidator().Validate(user);
            if (!resultValidator.IsValid)
                _helper.ValidatorHandler(resultValidator);
            if (oldUser.Password != user.Password)
                oldUser.Password = _authenticationService.GetHash(user.Password);
            oldUser.Document = user.Document;
            oldUser.Email = user.Email;
            oldUser.Name = user.Name;
            _userContext.Update(oldUser);
            return ConvertDTOToUser(oldUser);
        }

        public int DeleteUser(int id)
        {
            return _userContext.Remove(id);
        }

        public IList<UserDTO> List()
        {
            return _userContext.SelectAll();
        }

        public User GetById(int id)
        {
            var userDTO = _userContext.Select(id);
            if (userDTO != null)
                return ConvertDTOToUser(userDTO);

            return null;
        }

        private UserDTO ConvertUserToDTO(User user)
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

        private User ConvertDTOToUser(UserDTO user)
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
