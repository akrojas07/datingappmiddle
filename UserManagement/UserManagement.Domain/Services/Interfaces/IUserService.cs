using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models;

namespace UserManagement.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<long> CreateNewUser(UserModel newUser);
        Task<List<UserModel>> GetAllUsers();
        Task UpdateUserProfile(UserModel updatedUser);
        Task DeleteUserAccount(string userName);
        Task<string> Login(string userName, string password);
        Task Logout(string userName);
    }
}
