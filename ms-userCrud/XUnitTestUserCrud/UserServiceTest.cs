using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ms_userCrud._01Api.Model;
using ms_userCrud._01Api.Validator;
using ms_userCrud._02Service;
using ms_userCrud._02Service.Security;
using ms_userCrud._03Data;
using ms_userCrud._03Data.Entity;
using ms_userCrud._05Helper;
using ms_userCrud.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestUserCrud
{
    public class UserServiceTest
    {
        private UserService _userService;

        public UserServiceTest()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase("DataUserMemory");
            Environment.SetEnvironmentVariable("CryptographyKey", "CryptographySecretKeyValue");


            var tokenConfigurations = new TokenConfigurations();
            var signingConfigurations = new SigningConfigurations();
            var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                config.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

            var dbContext = new DBContext(options.Options);

            var authenticationService = new AuthHelper(signingConfigurations, tokenConfigurations);
            var userAuthValidator = new UserAuthValidator();
            var helper = new Helper();
            var userContext = new UserContext<UserDTO>(dbContext);
            var userValidator = new UserValidator();

            _userService = new UserService(authenticationService, userAuthValidator, helper, userContext, userValidator);
        }

        [Fact]
        public void TestAuthenticate_OK()
        {

            _userService.InsertUser(GetUser01());
            var response = _userService.Authentication(GetUser01());

            Assert.NotNull(response.AccessToken);
        }

        [Fact]
        public void TestAuthenticate_NOTFOUND()
        {

            _userService.InsertUser(GetUser01());
            var user = GetUser01();
            user.Username = "usererror";
            var response = _userService.Authentication(user);

            Assert.Null(response);
        }

        [Fact]
        public void TestUserCreate_OK()
        {

            var response = _userService.InsertUser(GetUser01());

            Assert.Equal(1, response);
        }

        [Fact]
        public void TestUserCreate_ERROR()
        {
            var user = GetUser01();
            user.Username = null;

            Assert.Throws<Exception>(() => _userService.InsertUser(user));
        }

        [Fact]
        public void TestUpdate_OK()
        {

            var idResult = _userService.InsertUser(GetUser01());
            var userUpdated = GetUser01();
            userUpdated.Password = "Passchanged";
            userUpdated.Username = "UsernameChanged";
            var response = _userService.UpdateUser((int)idResult, userUpdated);

            Assert.NotNull(response);
        }

        [Fact]
        public void TestUpdate_ERROR()
        {

            var idResult = _userService.InsertUser(GetUser01());
            var userUpdated = GetUser01();
            userUpdated.Password = "Passchanged";
            userUpdated.Username = "UsernameChanged";
            var response = _userService.UpdateUser(10, userUpdated);

            Assert.Null(response);
        }

        [Fact]
        public void TestUpdate_InvalidEmail()
        {

            var idResult = _userService.InsertUser(GetUser01());
            var userUpdated = GetUser01();
            userUpdated.Email = "email@123";

            Assert.Throws<Exception>(() => _userService.UpdateUser(idResult, userUpdated));
        }

        [Fact]
        public void TestDelete_OK()
        {

            var idResult = _userService.InsertUser(GetUser01());
            var response = _userService.DeleteUser(idResult);

            Assert.Equal(1, response);
        }

        [Fact]
        public void TestDelete_ERROR()
        {

            var idResult = _userService.InsertUser(GetUser01());
            var response = _userService.DeleteUser(10);

            Assert.Equal(0, response);
        }

        [Fact]
        public void TestList_OK()
        {
            _userService.InsertUser(GetUser01());

            var response = _userService.List();

            Assert.Equal(GetUser01().Username, response[0].Username);
        }

        [Fact]
        public void TestUserById()
        {
            var idResult = _userService.InsertUser(GetUser01());
            var response = _userService.GetById(idResult);

            Assert.Equal(GetUser01().Username, response.Username);
        }

        public User GetUser01()
        {
            return new User()
            {
                Document = "000000001",
                Email = "1email@email.com",
                Name = "name01",
                Password = "password01",
                Username = "username01"
            };
        }
    }
}
