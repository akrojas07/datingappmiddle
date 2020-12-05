using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence.Interfaces;
using UserManagement.Infrastructure.Persistence.Entities;

namespace UserManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Repository method to create new user in db
        /// </summary>
        /// <param name="newUser">Repository User Entity</param>
        /// <returns>User Id as long</returns>
        public async Task<long> CreateNewUser(User newUser)
        {

            using (var context = new DatingAppContext())
            {
                if(newUser == null)
                {
                    throw new Exception("User not provided");
                }

                newUser.Status = true;
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;

                context.Users.Add(newUser);
                await context.SaveChangesAsync();
                return newUser.Id; 
            }
        }

        /// <summary>
        /// Repository method to delete existing user account 
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Completed Task</returns>
        public async Task DeleteUserAccount(long userId)
        {
            using(var context = new DatingAppContext())
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                context.Users.Remove(existingUser);
                await context.SaveChangesAsync();
              
                    
            }
        }

        /// <summary>
        /// Repository method to pull all users from database
        /// </summary>
        /// <returns>List of Users Entity</returns>
        public async Task<List<User>> GetAllUsers()
        {
            using (var context = new DatingAppContext())
            {
                List<User> allUsers = new List<User>();

                allUsers = await context.Users.ToListAsync();

                return allUsers;
            }
        }

        /// <summary>
        /// Repository method to pull individual user from database 
        /// </summary>
        /// <returns>User Entity</returns>
        public async Task<User> GetUserByUserName(string userName)
        {
            using(var context = new DatingAppContext())
            {
                User existingUser = new User();
                existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == userName);

                return existingUser;
            }
        }

        /// <summary>
        /// Repository method to update a user's login / logout status 
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Completed Task</returns>
        public async Task UpdateUserStatus(long userId, bool status)
        {
            using(var context = new DatingAppContext())
            {
                //pull user account
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                
                //update status 
                user.Status = status;

                await context.SaveChangesAsync();

            }
        }


        /// <summary>
        /// Repository method to update individual user's profile
        /// </summary>
        /// <param name="updatedUser">Repository User Entity</param>
        /// <returns>Completed Task</returns>

        public async Task UpdateUserProfile(User updatedUser)
        {
            using (var context = new DatingAppContext())
            {
                var userProfile = await context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

                userProfile.FirstName = updatedUser.FirstName;
                userProfile.LastName = updatedUser.LastName;
                userProfile.Password = updatedUser.Password;
                userProfile.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
    }
}
