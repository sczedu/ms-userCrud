using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Moq;
using ms_userCrud;
using ms_userCrud.Business;
using ms_userCrud.Controllers;
using ms_userCrud.Data;
using ms_userCrud.Models;
using ms_userCrud.Security;
using ms_userCrud.Security.SecurityClasses;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestUserCrud
{
    public class UserUnitTest
    {

        private UserController userController;
        private PasswordService passwordService;

        public UserUnitTest()
        {
            var options = new DbContextOptionsBuilder<MysqlDBContext>()
                .UseInMemoryDatabase("DataUserMemory");
            Environment.SetEnvironmentVariable("CryptographyKey", "CryptographySecretKeyValue");


            var tokenConfigurations = new TokenConfigurations();
            var signingConfigurations = new SigningConfigurations();
            var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                config.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

            var mysqlDBContext = new MysqlDBContext(options.Options);
            userController = new UserController(mysqlDBContext);
            passwordService = new PasswordService(signingConfigurations, tokenConfigurations);
            IdentityModelEventSource.ShowPII = true;
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

        [Fact]
        public async Task TestAuthenticate_OK()
        {

            userController.InsertUser(GetUser01(), passwordService);
            var response = userController.Authentication(GetUser01(), passwordService) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestAuthenticate_NOTFOUND()
        {

            userController.InsertUser(GetUser01(), passwordService);
            var user = GetUser01();
            user.Username = "usererror";
            var response = userController.Authentication(user, passwordService) as  NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task TestUserCreate_OK()
        {

            var response = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestUserCreate_ERROR()
        {
            var user = GetUser01();
            user.Username = null;
            var response = userController.InsertUser(user, passwordService) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_OK()
        {

            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var userUpdated = GetUser01();
            userUpdated.Password = "Passchanged";
            userUpdated.Username = "UsernameChanged"; 
            var response = userController.Update((int)idResult.Value, userUpdated, passwordService) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_ERROR()
        {

            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var userUpdated = GetUser01();
            userUpdated.Password = "Passchanged";
            userUpdated.Username = "UsernameChanged";
            var response = userController.Update(10, userUpdated, passwordService) as NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_InvalidEmail()
        {

            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var userUpdated = GetUser01();
            userUpdated.Email = "email@123";
            var response = userController.Update((int)idResult.Value, userUpdated, passwordService) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task TestDelete_OK()
        {

            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var response = userController.Delete((int)idResult.Value) as NoContentResult;

            Assert.Equal(204, response.StatusCode);
        }

        [Fact]
        public async Task TestDelete_ERROR()
        {

            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var response = userController.Delete(10) as NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task TestList_OK()
        {

            var response = userController.List() as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestUserById()
        {
            var idResult = userController.InsertUser(GetUser01(), passwordService) as OkObjectResult;
            var response = userController.Get((int)idResult.Value) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        
    }
}
