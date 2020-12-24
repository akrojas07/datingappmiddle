using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence.Interfaces;
using UserManagement.Infrastructure.Persistence.Entities;
using System.Linq;

namespace UserManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Repository method to create new user in db
        /// </summary>
        /// <param name="newUser">Repository User Entity</param>
        /// <returns>User Entity</returns>
        public async Task<User> CreateNewUser(User newUser)
        {
            using (var context = new DatingAppContext())
            {
                //validate that the new user entity isn't empty
                if(newUser == null)
                {
                    throw new Exception("User not provided");
                }

                //update status to be true and created / updates dates 
                newUser.Status = true;
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;

                context.Users.Add(newUser);

                await context.SaveChangesAsync();


                //pull user that has just been created
                var createdUser = await context.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
                return createdUser;
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
        /// Repository method to pull users by location
        /// </summary>
        /// <param name="location">Location as string</param>
        /// <returns>List of User Entities</returns>
        public async Task<List<User>> GetUsersByLocation(string location)
        {
            using(var context = new DatingAppContext())
            {
                return await context.Users.Where(u => u.Location == location)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// Repository method to pull list of users from database by userId
        /// </summary>
        /// <param name="userIds">List of User Ids</param>
        /// <returns>List of Users Entity</returns>
        public async Task<List<User>> GetUsersByUserId(List<long> userIds)
        {
            using (var context = new DatingAppContext())
            {
                return await context.Users
                    .Where(u => userIds.Any(id => u.Id == id)).ToListAsync();
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
                existingUser = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == userName);

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
                var userProfile = await context.Users.FirstOrDefaultAsync(u => u.Username == updatedUser.Username);

                userProfile.FirstName = updatedUser.FirstName;
                userProfile.LastName = updatedUser.LastName;
                userProfile.Password = updatedUser.Password;
                userProfile.Location = updatedUser.Location;
                userProfile.Gender = updatedUser.Gender;
                userProfile.About = updatedUser.About;
                userProfile.Interests = updatedUser.Interests;
                userProfile.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
    }
}
