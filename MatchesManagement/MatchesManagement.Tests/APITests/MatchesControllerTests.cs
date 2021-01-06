using System;
using System.Collections.Generic;


using NUnit.Framework;
using Moq;
using MatchesManagement.Domain.Interfaces;
using DomainUser = MatchesManagement.Domain.Models.User;
using DomainMatch = MatchesManagement.Domain.Models.Match;
using System.Threading.Tasks;
using MatchesManagement.API.Controllers;
using MatchesManagement.API.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchesManagement.Tests.APITests
{
    [TestFixture]
    public class MatchesControllerTests
    {
        private Mock<IMatchesServices> _matchesServices;

        [SetUp]
        public void Setup()
        {
            _matchesServices = new Mock<IMatchesServices>();
        }

        [Test]
        public async Task Test_GetNewMatches_Succes()
        {
            _matchesServices.Setup(m => m.GetNewPotentialMatches(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()))
                .ReturnsAsync(new List<DomainUser>() { });

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetNewMatches(1, "San Diego");

            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetNewMatches_BadRequest_EmptyUserId()
        {
            var controller = new MatchesController(_matchesServices.Object);

            var response = await controller.GetNewMatches(0, "San Diego");

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetNewMatches_BadRequest_EmptyLocation()
        {
            var controller = new MatchesController(_matchesServices.Object);

            var response = await controller.GetNewMatches(1, "");

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetNewMatches_InternalError()
        {
            _matchesServices.Setup(m => m.GetNewPotentialMatches(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()))
            .ThrowsAsync(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetNewMatches(1, "San Diego");

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }


        [Test]
        public async Task Test_GetMatchesByMatchId_Success()
        {
            _matchesServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new DomainMatch() { Id = 1, Matched = true, Liked = true, FirstUserId = 2, SecondUserId = 4});

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetMatchesByMatchId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetMatchesByMatchId_BadRequest_NoIdProvided()
        {
            var controller = new MatchesController(_matchesServices.Object);

            var response = await controller.GetMatchesByMatchId(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetMatchesByMatchId_InternalError()
        {
            _matchesServices.Setup(m => m.GetMatchByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(""));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetMatchesByMatchId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetMatchesByUserId_Success()
        {
            _matchesServices.Setup(m => m.GetMatchesByUserId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new List<DomainMatch>() 
                {
                    {new DomainMatch(){ Id = 1, Matched = true, Liked = true, FirstUserId = 2, SecondUserId = 4} }
                });

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetMatchesByUserId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetMatchesByUserId_Fail_BadRequest_EmptyUserId()
        {
            var controller = new MatchesController(_matchesServices.Object);

            var response = await controller.GetMatchesByUserId(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetMatchesByUserId_Fail_InternalError()
        {
            _matchesServices.Setup(m => m.GetMatchesByUserId(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(""));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetMatchesByUserId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_UpsertMatches_Success()
        {
            _matchesServices.Setup(m => m.UpsertMatches(It.IsAny<List<DomainMatch>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.UpsertMatches(new UpsertMatchesRequest()
            {
                UpsertMatches = new List<UpsertMatch>() 
                {
                    {new UpsertMatch() {Id = 1, Matched = true, Liked = true, FirstUserId = 2, SecondUserId = 4 } }
                }
            });
            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((StatusCodeResult)response).StatusCode);

        }

        [Test]
        public async Task Test_UpsertMatches_Fail_BadRequest()
        {
            var controller = new MatchesController(_matchesServices.Object);
            var response = await controller.UpsertMatches(null);
            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((StatusCodeResult)response).StatusCode);
        }

        [Test]
        public async Task Test_UpsertMatches_Fail_InternalError()
        {
            _matchesServices.Setup(m => m.UpsertMatches(It.IsAny<List<DomainMatch>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new MatchesController(_matchesServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.UpsertMatches(new UpsertMatchesRequest() { });
            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }





















    }
}
