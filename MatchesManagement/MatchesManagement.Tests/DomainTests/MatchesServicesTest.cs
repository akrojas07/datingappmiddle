using System;
using System.Collections.Generic;
using System.Text;

using Moq;
using NUnit.Framework;
using MatchesManagement.Infrastructure.Persistence.Repositories.Interfaces;
using MatchesManagement.Infrastructure.Persistence.Entities;
using MatchesManagement.Infrastructure.UserManagementAPI.Interfaces;
using MatchesManagement.Infrastructure.UserManagementAPI.Models;
using UserServicesUser = MatchesManagement.Infrastructure.UserManagementAPI.Models.User;
using MatchesManagement.Domain.Services;
using DomainMatch = MatchesManagement.Domain.Models.Match;
using MatchesManagement.Domain.Models;

using System.Threading.Tasks;


namespace MatchesManagement.Tests.DomainTests
{
    [TestFixture]
    public class MatchesServicesTest
    {
        private Mock<IMatchesRepository> _matchesRepository;
        private Mock<IUserServices> _userServices;

        [SetUp]
        public void Setup()
        {
            _matchesRepository = new Mock<IMatchesRepository>();
            _userServices = new Mock<IUserServices>();
        }

        [Test]
        public async Task Test_GetMatchByMatchId_Success()
        {
            _matchesRepository.Setup(u => u.GetMatchByMatchId(It.IsAny<long>()))
                .ReturnsAsync(new Matches()
                {
                    MatchId = 1, FirstUserId = 1, SecondUserId = 2, Liked = true, Matched = false
                });

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            await matchesServices.GetMatchByMatchId(1, "token");

            _matchesRepository.Verify(u => u.GetMatchByMatchId(It.IsAny<long>()), Times.Once);

        }

        [Test]
        public void Test_GetMatchByMatchId_Fail_InvalidId()
        {
            _matchesRepository.Setup(u => u.GetMatchByMatchId(It.IsAny<long>()));

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            Assert.ThrowsAsync<Exception>(() => matchesServices.GetMatchByMatchId(1, "token"));

            _matchesRepository.Verify(u => u.GetMatchByMatchId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Test_GetMatchByMatchId_Fail_BadRequest()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.GetMatchByMatchId(0, "token"));

            _matchesRepository.Verify(u => u.GetMatchByMatchId(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public async Task Test_GetMatchesByUserId_Success()
        {
            _matchesRepository.Setup(m => m.GetMatchesByUserId(It.IsAny<long>()))
                .ReturnsAsync(new List<Matches>() 
                {
                    { new Matches(){MatchId = 1, FirstUserId = 1, SecondUserId = 2, Liked = true, Matched = true } }
                });

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            await matchesServices.GetMatchesByUserId(1, "token");

            _matchesRepository.Verify(m => m.GetMatchesByUserId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Test_GetMatchesByUserId_Fail_BadRequest()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.GetMatchesByUserId(0, "token"));

            _matchesRepository.Verify(m => m.GetMatchesByUserId(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void Test_GetMatchesByUserId_Fail_InternalError()
        {
            _matchesRepository.Setup(m => m.GetMatchesByUserId(It.IsAny<long>()));

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);

            Assert.ThrowsAsync<Exception>(() => matchesServices.GetMatchesByUserId(1, "token"));

            _matchesRepository.Verify(m => m.GetMatchesByUserId(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Test_GetNewPotentialMatches_Success()
        {
            _userServices.Setup(u => u.GetUsersByLocation(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<UserServicesUser>()
                {
                    { new UserServicesUser(){ Id = 5, About = "", FirstName = "firstname", LastName = "", Location = "San Diego", Gender = true, Interests = "", Username = "username"} }
                }) ;

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            await matchesServices.GetNewPotentialMatches("San Diego", "token", 1);

            _userServices.Verify(u => u.GetUsersByLocation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Test_GetNewPotentialMatches_Success_NoMatches()
        {
            _userServices.Setup(u => u.GetUsersByLocation(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<UserServicesUser>()
                {
                                { new UserServicesUser(){ Id = 1, About = "", FirstName = "firstname", LastName = "", Location = "San Diego", Gender = true, Interests = "", Username = "username"} }
                });

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            await matchesServices.GetNewPotentialMatches("San Diego", "token", 1);

            _userServices.Verify(u => u.GetUsersByLocation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_GetNewPotentialMatches_Fail_EmtpyLocation()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.GetNewPotentialMatches("", "token", 1));

            _userServices.Verify(u => u.GetUsersByLocation(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Test_UpsertMatches_Success()
        {
            _matchesRepository.Setup(m => m.UpsertMatches(It.IsAny<List<Matches>>()))
                .Returns(Task.CompletedTask);
            _matchesRepository.Setup(m => m.GetMatchByMatchId(It.IsAny<long>()))
                .ReturnsAsync(new Matches() 
                {
                    MatchId = 1,
                    FirstUserId = 1,
                    SecondUserId = 2,
                    Liked = true
                });
            _userServices.Setup(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<UserServicesUser>() 
                {
                    {new UserServicesUser(){ Id = 1} },
                    {new UserServicesUser(){ Id = 2} }
                });

            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            await matchesServices.UpsertMatches(new List<DomainMatch>()
            {
                {new DomainMatch(){ Id = 5, FirstUserId = 1, SecondUserId = 2, Liked = true} }
            }, "token");

            _matchesRepository.Verify(m => m.UpsertMatches(It.IsAny<List<Matches>>()), Times.Once);
            _matchesRepository.Verify(m => m.GetMatchByMatchId(It.IsAny<long>()), Times.AtLeastOnce);
            _userServices.Verify(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()), Times.AtLeastOnce);
            
        }

        [Test]
        public void Test_UpsertMatches_BadRequest_NullToken()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.UpsertMatches(new List<DomainMatch>()
            {
                {new DomainMatch(){ Id = 5, FirstUserId = 1, SecondUserId = 2, Liked = true} }
            }, null));

            _matchesRepository.Verify(m => m.UpsertMatches(It.IsAny<List<Matches>>()), Times.Never);
            _matchesRepository.Verify(m => m.GetMatchByMatchId(It.IsAny<long>()), Times.Never);
            _userServices.Verify(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Test_UpsertMatches_BadRequest_NullList()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.UpsertMatches(null, "token"));

            _matchesRepository.Verify(m => m.UpsertMatches(It.IsAny<List<Matches>>()), Times.Never);
            _matchesRepository.Verify(m => m.GetMatchByMatchId(It.IsAny<long>()), Times.Never);
            _userServices.Verify(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Test_UpsertMatches_BadRequest_EmptyList()
        {
            var matchesServices = new MatchesServices(_matchesRepository.Object, _userServices.Object);
            Assert.ThrowsAsync<ArgumentException>(() => matchesServices.UpsertMatches(new List<DomainMatch>(), "token"));

            _matchesRepository.Verify(m => m.UpsertMatches(It.IsAny<List<Matches>>()), Times.Never);
            _matchesRepository.Verify(m => m.GetMatchByMatchId(It.IsAny<long>()), Times.Never);
            _userServices.Verify(u => u.GetUsersByUserId(It.IsAny<List<long>>(), It.IsAny<string>()), Times.Never);
        }

    }
}
