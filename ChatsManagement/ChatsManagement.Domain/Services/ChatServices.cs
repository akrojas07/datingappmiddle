using ChatsManagement.Domain.Models;
using ChatsManagement.Domain.Services.Interfaces;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatsManagement.Domain.Mapper;
using ChatsManagement.Infrastructure.UserManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.Persistence.Entities;
using UMUser = ChatsManagement.Infrastructure.UserManagementAPI.Models.User;
using System.Linq;

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
            if (newChat == null)
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
            if (chatId <= 0)
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
            if (matchId <= 0)
            {
                throw new ArgumentException();
            }

            List<DomainChat> domainChats = new List<DomainChat>();

            //validate matchId from match service
            var existingMatch = await _matchServices.GetMatchByMatchId(matchId, token);

            if (existingMatch == null)
            {
                throw new Exception("Match does not exist");
            }

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByMatchId(matchId);

            //if chats exist, validate users exist
            if (dbChats != null && dbChats.Count > 0)
            {
                //validate users
                var userIdList = ValidateUsers(dbChats);

                //pull user information based on user id list
                List<UMUser> userInfoList = await _userService.GetUsersByUserId(userIdList, token);

                //loop through return db chats and user information list to map db chats to domain chats with user information
                foreach (var dbChat in dbChats)
                {
                    var chat = EFChatsMapper.DbToDomainChat(dbChat);

                    chat.FirstUsername = UserManagementToChatUserMapper.UMUserToDomainChat(userInfoList, chat.FirstUserId);
                    chat.SecondUsername = UserManagementToChatUserMapper.UMUserToDomainChat(userInfoList, chat.SecondUserId);
                    domainChats.Add(chat);
                }
            }

            //return list of domain chats
            return domainChats;

        }

        public async Task<List<DomainChat>> GetChatsByUserId(long userId, string token)
        {
            if (userId <= 0)
            {
                throw new ArgumentException();
            }

            List<DomainChat> domainChats = new List<DomainChat>();
            List<long> userIdList = new List<long>();

            userIdList.Add(userId);

            //validate user
            var existingUser = await _userService.GetUsersByUserId(userIdList, token);

            if (existingUser.Count <= 0)
            {
                throw new Exception("User does not exist");
            }

            //pull chats from db
            var dbChats = await _chatRepository.GetChatsByUserId(userId);

            //if chats exist, validate users exist
            if (dbChats != null && dbChats.Count > 0)
            {
                //validate users
                userIdList = ValidateUsers(dbChats, userId);

                //pull user information based on user id list
                List<UMUser> userInfoList = await _userService.GetUsersByUserId(userIdList, token);

                //loop through return db chats and user information list to map db chats to domain chats with user information
                foreach (var dbChat in dbChats)
                {
                    var chat = EFChatsMapper.DbToDomainChat(dbChat);

                    chat.FirstUsername = UserManagementToChatUserMapper.UMUserToDomainChat(userInfoList, chat.FirstUserId);
                    chat.SecondUsername = UserManagementToChatUserMapper.UMUserToDomainChat(userInfoList, chat.SecondUserId);
                    domainChats.Add(chat);
                }
            }

            //return list of domain chats
            return domainChats;

        }

        private List<long> ValidateUsers(List<Chat> dbChats, long userId = 0)
        {
            List<long> userIdList = new List<long>();

            if(userId > 0)
            {
                userIdList.Add(userId);
            }

            foreach (var dbChat in dbChats)
            {
                if (dbChat.FirstUserId != userId && !userIdList.Any(id => id == dbChat.FirstUserId))
                {
                    userIdList.Add(dbChat.FirstUserId);
                }
                
                if (dbChat.SecondUserId != userId && !userIdList.Any(id => id == dbChat.SecondUserId))
                {
                    userIdList.Add(dbChat.SecondUserId);
                }

                    continue;

            }

            return userIdList;
        }
    }
}
