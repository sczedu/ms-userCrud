using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ms_userCrud._01Api.Model;
using ms_userCrud._02Service;
using ms_userCrud._03Data.Entity;
using ms_userCrud.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestUserCrud
{
    public class UserControllerTeste
    {
        private Mock<IMapper> _mapper;
        private Mock<IUserService> _serviceUser;
        private UserController _userController;


        public UserControllerTeste()
        {
            _mapper = new Mock<IMapper>();
            _serviceUser = new Mock<IUserService>();
            _userController = new UserController(_serviceUser.Object);
        }

        [Fact]
        public async Task TestAuthenticate_OK()
        {
            var newUser = Builder<User>.CreateNew().Build();
            var newToken = Builder<Token>.CreateNew().Build();
            _serviceUser.Setup(s => s.Authentication(It.IsAny<User>())).Returns(newToken);
            var result = _userController.Authentication(newUser) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);

        }

        [Fact]
        public async Task TestAuthenticate_NOTFOUND()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.InsertUser(It.IsAny<User>())).Returns(0);
            var result = _userController.InsertUser(newUser) as BadRequestResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task TestUserCreate_OK()
        {

            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.InsertUser(It.IsAny<User>())).Returns(1);
            var result = _userController.InsertUser(newUser) as CreatedResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task TestUserCreate_ERROR()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.InsertUser(It.IsAny<User>())).Returns(0);
            var result = _userController.InsertUser(newUser) as BadRequestResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_OK()
        {

            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns(newUser);
            _serviceUser.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).Returns(newUser);
            var result = _userController.Update(It.IsAny<int>(), Builder<User>.CreateNew().Build()) as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_ERROR()
        {

            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns(newUser);
            _serviceUser.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).Returns((User)null);
            var result = _userController.Update(It.IsAny<int>(), Builder<User>.CreateNew().Build()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_OK()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns(newUser);
            _serviceUser.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(1);
            var result = _userController.Delete(newUser.Id) as NoContentResult;

            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_ERROR()
        {

            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns((User)null);
            _serviceUser.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(1);
            var result = _userController.Delete(newUser.Id) as NotFoundResult;

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task TestList_OK()
        {

            var newListUser = Builder<List<UserDTO>>.CreateNew().Build();
            _serviceUser.Setup(s => s.List()).Returns(newListUser);
            var result = _userController.List() as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task TestUserById()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns(newUser);
            var result = _userController.Get(It.IsAny<int>()) as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task TestUserById_NotFound()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.GetById(It.IsAny<int>())).Returns((User)null);
            var result = _userController.Get(It.IsAny<int>()) as NotFoundResult;

            Assert.Equal(404, result.StatusCode);
        }


    }
}