using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Moq;

using UserManagement.API.Controllers;
using UserManagement.API.Models;
using UserManagement.Domain.Services.Interfaces;
using UserManagement.Domain.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Test.APITests
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IUserService> _userService;
        private Mock<IConfiguration> _config;


        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
            _config = new Mock<IConfiguration>();

            _config.Setup(c => c.GetSection(It.Is<string>(s => s.Equals("Jwt:Key"))).Value)
                .Returns("ewhsacvopturopgnew5tmsh9w9pjvg0a3syz5px9sjbo7cz17g");

            _config.Setup(c => c.GetSection(It.Is<string>(s => s.Equals("Jwt:Issuer"))).Value)
                .Returns("issuer.com");
        }

        [Test]
        public async Task Test_CreateNewUserController_Success()
        {
            _userService.Setup(u => u.CreateNewUser(It.IsAny<UserModel>()))
                .ReturnsAsync("username");

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.CreateNewUser(new NewUserRequest()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Username = "username",
                Password = "password"
            });

            Assert.NotNull(response);
            Assert.AreEqual(201, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_CreateNewUserController_Fail_InvalidUser()
        {
            _userService.Setup(u => u.CreateNewUser(It.IsAny<UserModel>()));

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.CreateNewUser(null);

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_CreateNewUserController_Fail_InternalError()
        {
            _userService.Setup(u => u.CreateNewUser(It.IsAny<UserModel>()))
                .ThrowsAsync(new Exception("internal error"));

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.CreateNewUser(new NewUserRequest()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Username = "username",
                Password = "password"
            });

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Success()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser("username", null);

            Assert.NotNull(response);
            Assert.AreEqual(200, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Fail_WrongToken()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser("username", "55");

            Assert.NotNull(response);
            Assert.AreEqual(401, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Fail_NullUsername()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser(null , null);

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Fail_InternalError()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .ThrowsAsync(new Exception()) ;

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser("username", null);

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }
    }
}
