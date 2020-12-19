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
                .ReturnsAsync(new UserModel() { });

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
            var response = await controller.DeleteUser("username");

            Assert.NotNull(response);
            Assert.AreEqual(200, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Fail_NullUsername()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser(null);

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_DeleteUserAccountController_Fail_InternalError()
        {
            _userService.Setup(u => u.DeleteUserAccount(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.DeleteUser("username");

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetAllUsersByUserId_Success()
        {
            _userService.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>()))
                .ReturnsAsync(new List<UserModel>()
                {
                    new UserModel
                    {
                        Username="username",
                        FirstName ="firstname",
                        LastName = "lastname",
                        Password = "password",
                        Id = 1
                    },
                    new UserModel
                    {
                        Username="username",
                        FirstName ="firstname",
                        LastName = "lastname",
                        Password = "password",
                        Id = 2
                    }
                });

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.GetUsersByUserId(new GetUsersByUserIdRequest() {UserIds = new List<long>{1,2,3,4} });

            Assert.NotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetAllUsersByUserId_Fail_EmptyInputList()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.GetUsersByUserId(new GetUsersByUserIdRequest() { UserIds = new List<long> {} });

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetUsersByUserId_Fail_NoDbUsers()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.GetUsersByUserId(new GetUsersByUserIdRequest() { UserIds = new List<long> { 100, 200, 300, 400 } });

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_Login_Success()
        {
            _userService.Setup(u => u.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new UserModel() { Username="username"});

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Login(new LoginUserRequest() { Username = "username", Password = "password"});

            Assert.NotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_Login_Fail_NullUsernameInput()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Login(new LoginUserRequest() { Username = null, Password = "password" });

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_Login_Fail_InternalError()
        {
            _userService.Setup(u => u.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Exception"));

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Login(new LoginUserRequest() { Username = "username", Password = "password" });

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_Logout_Success()
        {
            _userService.Setup(u => u.Logout(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Logout(new LogoutRequest() { Username="username"});

            Assert.NotNull(response);
            Assert.AreEqual(200, ((OkResult)response).StatusCode);
        }

        [Test]
        public async Task Test_Logout_Fail_NullUsername()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Logout(new LogoutRequest());

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_Logout_Fail_InternalError()
        {
            _userService.Setup(u => u.Logout(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Internal Exception"));

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.Logout(new LogoutRequest() { Username = "username" });

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_UpdateProfile_Success()
        {
            _userService.Setup(u => u.UpdateUserProfile(It.IsAny<UserModel>()))
                .Returns(Task.CompletedTask);

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.UpdateProfile(new UpdateUserRequest()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Username= "username",
                Password = "password"
            });

            Assert.NotNull(response);
            Assert.AreEqual(200, ((OkResult)response).StatusCode);
        }

        [Test]
        public async Task Test_UpdateProfile_Fail_NullInputs()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.UpdateProfile(null);

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_UpdateProfile_Fail_NullRequiredInput()
        {
            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.UpdateProfile(new UpdateUserRequest()
            {
                FirstName = "firstname",
                Username = "username",
                Password = "password"
            });

            Assert.NotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_UpdateProfile_Fail_InternalError()
        {
            _userService.Setup(u => u.UpdateUserProfile(It.IsAny<UserModel>()))
                .ThrowsAsync(new Exception("Internal Exception"));

            var controller = new UserController(_userService.Object, _config.Object);
            var response = await controller.UpdateProfile(new UpdateUserRequest()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Username = "username",
                Password = "password"
            });

            Assert.NotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }
    }
}
