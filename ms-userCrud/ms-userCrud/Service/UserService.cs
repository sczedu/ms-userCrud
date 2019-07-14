using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ms_userCrud.Api.Model;
using ms_userCrud.Api.Validator;
using ms_userCrud.Service.Mapper;
using ms_userCrud.Data;
using ms_userCrud.Data.Entity;
using ms_userCrud.Helper;
using System.Collections.Generic;
using System.Linq;

namespace ms_userCrud.Service
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

        public User InsertUser(User user)
        {
            var resultValidator = new UserValidator().Validate(user);
            if (!resultValidator.IsValid)
                _helper.ValidatorHandler(resultValidator);
            user.Password = _authenticationService.GetHash(user.Password);
            var userDTO = UserMapper.ConvertUserToDTO(user);
            if (_userContext.Insert(userDTO) == 1)
                return UserMapper.ConvertDTOToUser(_userContext.SelectAll().Where(w => w.Username == userDTO.Username).FirstOrDefault());
            return null;
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
            return UserMapper.ConvertDTOToUser(oldUser);
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
                return UserMapper.ConvertDTOToUser(userDTO);

            return null;
        }
    }
}
