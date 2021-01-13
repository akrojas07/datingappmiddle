using ChatsManagement.Domain.Models;
using ChatsManagement.Domain.Services.Interfaces;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatsManagement.Domain.Mapper;
using ChatsManagement.Infrastructure.UserManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces;

namespace ChatsManagement.Domain.Services
{
    public class ChatServices : IChatServices
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly IMatchServices _matchServices;

        public ChatServices(IChatRepository chatRepository, IUserService userService, IMatchServices matchServices)
        {
            _chatRepository = chatRepository;
            _userService = userService;
            _matchServices = matchServices;
        }

        /// <summary>
        /// Service method to add new chat message by match id 
        /// </summary>
        /// <param name="newChat">Domain Chat object</param>
        /// <returns>Task Complete</returns>
        public async Task<long> AddNewChatMessageByMatchId(DomainChat newChat, string token)
        {
            if(newChat == null)
            {
                throw new ArgumentException();
            }

            //validate match exists 
            var existingMatch = await _matchServices.GetMatchByMatchId(newChat.MatchId, token);

            if (existingMatch == null)
            {
                throw new Exception("Match does not exist");
            }

            var dbChat = EFChatsMapper.DomainToDbChat(newChat);

            var chatId = await _chatRepository.AddNewChatMessageByMatchId(dbChat);

            return chatId;
        }

        /// <summary>
        /// Service method to delete existing chat message by chat id
        /// </summary>
        /// <param name="chatId">Long Chat Id</param>
        /// <returns>Task Complete</returns>

        public async Task DeleteExistingChatMessage(long chatId)
        {
            if(chatId <= 0)
            {
                throw new ArgumentException();
            }

            await _chatRepository.DeleteExistingChatMessage(chatId);
        }


        /// <summary>
        /// Service Method to pull chats by match id
        /// </summary>
        /// <param name="matchId">Long Match Id</param>
        /// <returns>List of Domain Chats</returns>
        public async Task<List<DomainChat>> GetChatsByMatchId(long matchId, string token)
        {
            //validate inputs 
            if(matchId <= 0)
            {
                throw new ArgumentException();
            }

            List<DomainChat> domainChats = new List<DomainChat>();

            //validate matchId from match service
            var existingMatch = await _matchServices.GetMatchByMatchId(matchId, token);

            if(existingMatch == null)
            {
                throw new Exception("Match does not exist");
            }

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByMatchId(matchId);

            //if chats exist, map db chats to domain chats 
            if(dbChats != null && dbChats.Count > 0)
            {
                foreach(var dbChat in dbChats)
                {
                    domainChats.Add(EFChatsMapper.DbToDomainChat(dbChat));
                }
            }

            //return list of domain chats
            return domainChats;

        }

        public async Task<List<DomainChat>> GetChatsByUserId(long userId, string token)
        {
            if(userId <=0)
            {
                throw new ArgumentException();
            }
            
            List<DomainChat> domainChats = new List<DomainChat>();
            List<long> userIdList = new List<long>();
            userIdList.Add(userId);

            //validate user from user service
            var existingUser = await _userService.GetUsersByUserId(userIdList, token);
            if(existingUser.Count <= 0)
            {
                throw new Exception("User does not exist");
            }

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByUserId(userId);

            //if chats exist, map db chats to domain chats 
            if (dbChats != null && dbChats.Count > 0)
            {
                foreach (var dbChat in dbChats)
                {
                    domainChats.Add(EFChatsMapper.DbToDomainChat(dbChat));
                }
            }

            //return list of domain chats
            return domainChats;

        }
    }
}
