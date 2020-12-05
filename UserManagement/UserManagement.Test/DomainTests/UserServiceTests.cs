using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

using UserManagement.Domain.Models;
using UserManagement.Domain.Services;

using UserManagement.Infrastructure.Persistence.Interfaces;
using UserManagement.Infrastructure.Persistence.Entities;

namespace UserManagement.Test.DomainTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
        }

        [Test]
        public async Task Test_CreateNewUser_Success()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()));
            _userRepository.Setup(u => u.CreateNewUser(It.IsAny<User>()))
                .ReturnsAsync(1);

            var userService = new UserService(_userRepository.Object);
            await userService.CreateNewUser(new UserModel()
            {
                FirstName = "Bonnie",
                LastName = "Clyde",
                Password = "BoonieandClyde",
                Username = "Forever"
            });

            _userRepository.Verify(u => u.CreateNewUser(It.IsAny<User>()), Times.Once);

        }

        [Test]
        public void Test_CreateNewUser_Fail_ExistingUser()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever"
                });


            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.CreateNewUser(new UserModel()
            {
                FirstName = "Bonnie",
                LastName = "Clyde",
                Password = "BoonieandClyde",
                Username = "Forever"
            }));

            _userRepository.Verify(u => u.CreateNewUser(It.IsAny<User>()), Times.Never);

        }

        [Test]
        public void Test_CreateNewUser_Fail_InvalidInput()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()));

            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.CreateNewUser(new UserModel()
            {
                FirstName = "Clyde",
                LastName = "BoonieandClyde",
                Username = "Forever"
            }));

            _userRepository.Verify(u => u.CreateNewUser(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void Test_CreateNewUser_Fail_NullInput()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()));

            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.CreateNewUser(null));

            _userRepository.Verify(u => u.CreateNewUser(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public async Task Test_DeleteUserAccount_Success()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever"
                });
            _userRepository.Setup(u => u.DeleteUserAccount(It.IsAny<long>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(_userRepository.Object);
            await userService.DeleteUserAccount("Forever");

            _userRepository.Verify(u => u.DeleteUserAccount(It.IsAny<long>()), Times.Once);

        }


        [Test]
        public void Test_DeleteUserAccount_Fail_NullInput()
        {
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.DeleteUserAccount(null));

            _userRepository.Verify(u => u.DeleteUserAccount(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_DeleteUserAccount_Fail_InvalidInput()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()));

            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.DeleteUserAccount("Chonker"));

            _userRepository.Verify(u => u.DeleteUserAccount(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public async Task Test_GetAllUsers_Success()
        {
            _userRepository.Setup(u => u.GetAllUsers())
                .ReturnsAsync(new List<User>()
                {
                    {new User(){ FirstName = "Boonie", LastName="Cl"} }
                });

            var userService = new UserService(_userRepository.Object);
            await userService.GetAllUsers();

            _userRepository.Verify(u => u.GetAllUsers(), Times.Once);
        }

        [Test]
        public void Test_GetAllUsers_Fail_NoUsers()
        {
            _userRepository.Setup(u => u.GetAllUsers());
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.GetAllUsers());

            _userRepository.Verify(u => u.GetAllUsers(), Times.Once);
        }

        [Test]
        public async Task Test_Login_Success()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever",
                    Status = false
                });
            _userRepository.Setup(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(_userRepository.Object);
            await userService.Login("Forever", "BoonieandClyde");

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

        }

        [Test]
        public void Test_Login_Fail_InvalidInput()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
            .ReturnsAsync(new User()
            {
                FirstName = "Bonnie",
                LastName = "Clyde",
                Password = "BoonieandClyde",
                Username = "Forever",
                Status = false
            });
            _userRepository.Setup(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Login("Forever", "boonieandClyde"));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void Test_Login_Fail_NullInput()
        {
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Login("Forever", null));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void Test_Login_Fail_NoUserFound()
        {
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Login("Forever", "BoonieandClyde"));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public async Task Test_Logout_Success()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever",
                    Status = false
                });
            _userRepository.Setup(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(_userRepository.Object);
            await userService.Logout("Forever");

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

        }

        [Test]
        public void Test_Logout_Fail_InvalidInput()
        {
            _userRepository.Setup(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Logout("orever"));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void Test_Logout_Fail_NullInput()
        {
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Logout(null));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void Test_Logout_Fail_NoUserFound()
        {
            var userService = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => userService.Logout("Forever"));

            _userRepository.Verify(u => u.UpdateUserStatus(It.IsAny<long>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public async Task Test_UpdateUserProfile_Success()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever",
                    Status = true
                });
            _userRepository.Setup(u => u.UpdateUserProfile(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var service = new UserService(_userRepository.Object);
            await service.UpdateUserProfile(new UserModel() 
            {
                FirstName = "Bonkers",
                LastName = "Clyde",
                Password = "BoonieandClyde",
                Username = "Forever"
            });

            _userRepository.Verify(u => u.UpdateUserProfile(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void Test_UpdateUserProfile_Fail_InvalidInput()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    FirstName = "Bonnie",
                    LastName = "Clyde",
                    Password = "BoonieandClyde",
                    Username = "Forever",
                    Status = true
                });
            _userRepository.Setup(u => u.UpdateUserProfile(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var service = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => service.UpdateUserProfile(new UserModel()
            {
                FirstName = "Bonkers",
                LastName = "Clyde"
            }));

            _userRepository.Verify(u => u.UpdateUserProfile(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void Test_UpdateUserProfile_Fail_InvalidUser()
        {
            _userRepository.Setup(u => u.GetUserByUserName(It.IsAny<string>()));

            var service = new UserService(_userRepository.Object);
            Assert.ThrowsAsync<Exception>(() => service.UpdateUserProfile(new UserModel()
            {
                FirstName = "Bonkers",
                LastName = "Clyde",
                Password = "BoonieandClyde",
                Username = "Forever"
            }));

            _userRepository.Verify(u => u.UpdateUserProfile(It.IsAny<User>()), Times.Never);
        }
    }
}
