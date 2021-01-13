using ChatsManagement.Infrastructure.UserManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatsManagement.Infrastructure.UserManagementAPI.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersByUserId(List<long> userIds, string token);
    }
}
