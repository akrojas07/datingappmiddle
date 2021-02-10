using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Moq;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using ChatsManagement.Infrastructure.UserManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces;
using MMatch = ChatsManagement.Infrastructure.MatchesManagementAPI.Models.Match;
using DbChat = ChatsManagement.Infrastructure.Persistence.Entities.Chat;
using ChatsManagement.Infrastructure.UserManagementAPI.Models;

using System.Threading.Tasks;
using ChatsManagement.Domain.Services;

namespace ChatsManagement.Tests.DomainTests
{
    [TestFixture]
    public class ChatServicesTests
    {
        private Mock<IChatRepository> _chatRepository;
        private Mock<IUserService> _userService;
        private Mock<IMatchServices> _matchServices;

        [SetUp]
        public void Setup()
        {
            _chatRepository = new Mock<IChatRepository>();
            _userService = new Mock<IUserService>();
            _matchServices = new Mock<IMatchServices>();
        }

        [Test]
        public async Task Test_AddNewChatMessageByMatchId_Success()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.AddNewChatMessageByMatchId(It.IsAny<DbChat>()))
                .ReturnsAsync(1);

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);

            await chatServices.AddNewChatMessageByMatchId(new Domain.Models.DomainChat()
            {
                MatchId = 5,
                FirstUserId = 4,
                SecondUserId = 6,
                Message = "Message"
            },
            "token");

            _chatRepository.Verify(c => c.AddNewChatMessageByMatchId(It.IsAny<DbChat>()), Times.Once);

        }

        [Test]
        public void Test_AddNewChatMessageByMatchId_Fail_InvalidMatchId()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()));

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);

            Assert.ThrowsAsync<Exception>(() => chatServices.AddNewChatMessageByMatchId(new Domain.Models.DomainChat()
            {
                FirstUserId = 4,
                SecondUserId = 6,
                Message = "Message"
            },
            "token"));

            _chatRepository.Verify(c => c.AddNewChatMessageByMatchId(It.IsAny<DbChat>()), Times.Never);
        }

        [Test]
        public void Test_AddNewChatMessageByMatchId_Fail_ArgumentException()
        {
            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => chatServices.AddNewChatMessageByMatchId(null, "token"));
            _chatRepository.Verify(c => c.AddNewChatMessageByMatchId(It.IsAny<DbChat>()), Times.Never);
        }

        [Test]
        public async Task Test_GetChatsByMatchId_Success()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByMatchId(It.IsAny<long>()))
                .ReturnsAsync(new List<DbChat>() { new DbChat()
                {
                    MatchId = 5,
                    FirstUserId = 4,
                    SecondUserId = 6,
                    Message = "Message",
                    DateSent = new DateTime()
                } }) ;
            _userService.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<User>() { new User() { 
                    FirstName = "Tester",
                    Location = "San Diego",
                    Username = "FirstUser",
                    Id = 1
                } });

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            await chatServices.GetChatsByMatchId(1, "token");

            _chatRepository.Verify(c => c.GetChatsByMatchId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Test_GetChatsByMatchId_Fail_BadArgument()
        {
            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<ArgumentException>( () => chatServices.GetChatsByMatchId(0, "token"));

            _chatRepository.Verify(c => c.GetChatsByMatchId(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_GetChatsByMatchId_Fail_InternalError_InvalidMatch()
        {
            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<Exception>(() => chatServices.GetChatsByMatchId(1, "token"));

            _chatRepository.Verify(c => c.GetChatsByMatchId(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_GetChatsByMatchId_Fail_InternalError_InvalidUsers()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByMatchId(It.IsAny<long>()))
                .ReturnsAsync(new List<DbChat>() { new DbChat()
                {
                    MatchId = 5,
                    FirstUserId = 4,
                    SecondUserId = 6,
                    Message = "Message",
                    DateSent = new DateTime()
                } });
            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<Exception>(() => chatServices.GetChatsByMatchId(1, "token"));

            _chatRepository.Verify(c => c.GetChatsByMatchId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Test_GetChatsByMatchId_Fail_InternalError_NoDbChats()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByMatchId(It.IsAny<long>()));

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<Exception>(() => chatServices.GetChatsByMatchId(1, "token"));

            _chatRepository.Verify(c => c.GetChatsByMatchId(It.IsAny<long>()), Times.Once);
        }


        [Test]
        public async Task Test_GetChatsByUserId_Success()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByUserId(It.IsAny<long>()))
                .ReturnsAsync(new List<DbChat>() { new DbChat()
                {
                    MatchId = 5,
                    FirstUserId = 4,
                    SecondUserId = 6,
                    Message = "Message",
                    DateSent = new DateTime()
                } });
            _userService.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<User>() { new User() {
                    FirstName = "Tester",
                    Location = "San Diego",
                    Username = "FirstUser",
                    Id = 1
                } });

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            await chatServices.GetChatsByUserId(1, "token");

            _chatRepository.Verify(c => c.GetChatsByUserId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Test_GetChatsByUserId_Fail_BadArgument()
        {
            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => chatServices.GetChatsByUserId(0, "token"));

            _chatRepository.Verify(c => c.GetChatsByUserId(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_GetChatsByUserId_Fail_InternalError_InvalidUsers()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByUserId(It.IsAny<long>()))
                .ReturnsAsync(new List<DbChat>() { new DbChat()
                {
                    MatchId = 5,
                    FirstUserId = 4,
                    SecondUserId = 6,
                    Message = "Message",
                    DateSent = new DateTime()
                } });
            _userService.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()));

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<Exception>(() => chatServices.GetChatsByUserId(1, "token"));

            _chatRepository.Verify(c => c.GetChatsByUserId(It.IsAny<long>()), Times.Never);

        }

        [Test]
        public void Test_GetChatsByUserId_Fail_InternalError_NoDbChats()
        {
            _matchServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new MMatch());
            _chatRepository.Setup(c => c.GetChatsByUserId(It.IsAny<long>()));
            _userService.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<User>() { new User() {
                    FirstName = "Tester",
                    Location = "San Diego",
                    Username = "FirstUser",
                    Id = 1
                } });

            var chatServices = new ChatServices(_chatRepository.Object, _userService.Object, _matchServices.Object);
            Assert.ThrowsAsync<Exception>(() => chatServices.GetChatsByUserId(1, "token"));

            _chatRepository.Verify(c => c.GetChatsByUserId(It.IsAny<long>()), Times.Once);
        }
    }
}
