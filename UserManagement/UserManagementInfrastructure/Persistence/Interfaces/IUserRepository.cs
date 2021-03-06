﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.Persistence.Entities;

namespace UserManagement.Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateNewUser(User newUser);
        Task<List<User>> GetUsersByLocation(string location);
        Task<List<User>> GetUsersByUserId(List<long> userIds);
        Task<User> GetUserByUserName(string userName);
        Task UpdateUserProfile(User updatedUser);
        Task DeleteUserAccount(long userId);
        Task UpdateUserStatus(long userId, bool status);
    }
}
