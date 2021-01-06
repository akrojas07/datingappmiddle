using MatchesManagement.Infrastructure.UserManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchesManagement.Infrastructure.UserManagementAPI.Interfaces
{
    public interface IUserServices
    {
        Task<List<User>> GetUsersByUserId(List<long> userIds, string token);
        Task<List<User>> GetUsersByLocation(string location, string token);
    }
}
