using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using ChatsManagement.Domain.Services.Interfaces;
using System.Threading.Tasks;
using ChatsManagement.API.Controllers;
using ChatsManagement.Domain.Models;
using ChatsManagement.API.Models;
using System.Collections.Generic;
using System;

namespace ChatsManagement.Tests.APITests
{
    [TestFixture]
    public class ChatControllerTests
    {
        private Mock<IChatServices> _chatServices;

        [SetUp]
        public void Setup()
        {
            _chatServices = new Mock<IChatServices>();
        }

        [Test]
        public async Task Test_AddNewChatMessageByMatchId_Success()
        {
            _chatServices.Setup(c => c.AddNewChatMessageByMatchId(It.IsAny<DomainChat>(), It.IsAny<string>()));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.AddNewChat(new AddNewChatRequest() 
            { 
                FirstUserId = 1,
                SecondUserId = 2,
                MatchId = 4,
                Message = "This is a message"
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(201, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_AddNewChatMessageByMatchId_Fail_BadArgument()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.AddNewChat(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_AddNewChatMessageByMatchId_Fail_InternalError()
        {
            _chatServices.Setup(c => c.AddNewChatMessageByMatchId(It.IsAny<DomainChat>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.AddNewChat(new AddNewChatRequest()
            {
                FirstUserId = 1,
                SecondUserId = 2,
                Message = "This is a message"
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetChatsByMatchId_Success()
        {
            _chatServices.Setup(c => c.GetChatsByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new List<DomainChat>() { new DomainChat()
                {
                    FirstUserId = 4,
                    SecondUserId = 5,
                    MatchId = 5,
                    Id = 2,
                    Message = "Message",
                    DateSent = new DateTime()
                } }) ;
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByMatchId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_GetChatsByMatchId_Fail_BadArgument()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByMatchId(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_GetChatsByMatchId_Fail_InternalError()
        {
            _chatServices.Setup(c => c.GetChatsByMatchId(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByMatchId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_GetChatsByUserId_Success()
        {
            _chatServices.Setup(c => c.GetChatsByUserId(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new List<DomainChat>() { new DomainChat()
                {
                    FirstUserId = 4,
                    SecondUserId = 5,
                    MatchId = 5,
                    Id = 2,
                    Message = "Message",
                    DateSent = new DateTime()
                } });

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByUserId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
        }

        [Test]
        public async Task Test_GetChatsByUserId_Fail_BadArgument()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByUserId(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(400, ((ObjectResult)response).StatusCode);

        }

        [Test]
        public async Task Test_GetChatsByUserId_Fail_InternalError()
        {
            _chatServices.Setup(c => c.GetChatsByUserId(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            var controller = new ChatController(_chatServices.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = await controller.GetChatsByUserId(1);

            Assert.IsNotNull(response);
            Assert.AreEqual(500, ((ObjectResult)response).StatusCode);

        }
    }
}
