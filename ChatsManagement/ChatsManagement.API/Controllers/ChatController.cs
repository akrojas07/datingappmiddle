﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ChatsManagement.Domain.Models;
using ChatsManagement.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ChatsManagement.API.Models;

namespace ChatsManagement.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatServices _chatServices;

        public ChatController(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }

        [HttpPost]
        [Route("newchat")]
        public async Task<IActionResult> AddNewChat([FromBody] AddNewChatRequest newChatRequest)
        {
            //validate input
            if(newChatRequest == null)
            {
                return StatusCode(400, "Bad Request");
            }

            try
            {
                DomainChat domainChat = new DomainChat()
                {
                    FirstUserId = newChatRequest.FirstUserId,
                    SecondUserId = newChatRequest.SecondUserId,
                    MatchId = newChatRequest.MatchId,
                    Message = newChatRequest.Message,
                    DateSent = DateTime.Now
                };

                await _chatServices.AddNewChatMessageByMatchId(domainChat);
                return StatusCode(201);

            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("chat/{chatId}")]
        public async Task<IActionResult> DeleteChat(long chatId)
        {
            //validate input
            if(chatId <= 0)
            {
                return StatusCode(400, "Bad Request");
            }

            try
            {
                await _chatServices.DeleteExistingChatMessage(chatId);
                return StatusCode(200);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet]
        [Route("getchats/match/{matchId}")]
        public async Task<IActionResult> GetChatsByMatchId(long matchId)
        {
            //validate input
            if(matchId <= 0)
            {
                return StatusCode(400, "Bad Request");
            }

            try
            {
                var chats = await _chatServices.GetChatsByMatchId(matchId);
                return StatusCode(200, chats);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("getchats/user/{userId}")]
        public async Task<IActionResult> GetChatsByUserId(long userId)
        {
            if(userId <= 0)
            {
                return StatusCode(400, "Bad Request");
            }

            try
            {
                var chats = await _chatServices.GetChatsByUserId(userId);
                return StatusCode(200, chats);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
