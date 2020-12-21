using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Models;
using UserManagement.Domain.Services.Interfaces;
using UserManagement.Domain.Mapper;
using UserManagement.Infrastructure.Persistence.Interfaces;
using UserManagement.Infrastructure.StockPhotoAPI.Interfaces;
using System.Linq;

namespace UserManagement.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IStockPhotoServices _stockPhoto;
        public UserService(IUserRepository userRepository, IStockPhotoServices stockPhoto) 
        {
            _userRepository = userRepository;
            _stockPhoto = stockPhoto;
        }

        /// <summary>
        /// Service method to Create new Core User
        /// </summary>
        /// <param name="newUser">User Model</param>
        /// <returns>User Id </returns>
        public async Task<UserModel> CreateNewUser(UserModel newUser)
        {
            //validate new user 
            bool validUser = ValidateUser(newUser);

            if (validUser)
            {
                //if user properties are valid
                try
                {
                    //check to see if user by user name exists 
                    var existingUser = await _userRepository.GetUserByUserName(newUser.Username);
                    if(existingUser == null)
                    {
                        throw new Exception("User does not exist");
                    }
                }
                catch
                {
                    //if user doesn't exist, create new user 
                    var createdUser = await _userRepository.CreateNewUser(EfUserMapper.CoreModelToDbEntity(newUser));                    

                    //map db user to domain user
                    var newDomainUser = EfUserMapper.DbEntityToCoreModel(createdUser);

                    //map photo id to photo for domain user
                    if(createdUser.PhotoId != null)
                    {
                        var photo = await _stockPhoto.GetPhotoById((long)createdUser.PhotoId);
                        newDomainUser.Photo = new DomainPhoto() { Id = photo.Id, URL = photo.Source.Medium};
                    }

                    //return newly created user
                    return newDomainUser;
                }

                // if user exists, throw exception
                throw new Exception("Username already exists");
            }
            else
            {
                //if input isn't valid, throw exception
                throw new Exception("Invalid User");
            }
        }

        /// <summary>
        /// Service method to delete individual user account
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Completed task</returns>
        public async Task DeleteUserAccount(string userName)
        {
            //validate input
            if (userName == null)
            {
                throw new Exception("User not provided");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate return
            if(user == null)
            {
                throw new Exception("User not found");
            }

            //delete user account
            await _userRepository.DeleteUserAccount(user.Id);
        }

        /// <summary>
        /// Service method to pull single user by username
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <returns>Domain Model User</returns>
        public async Task<UserModel> GetUserByUsername(string username)
        {
            //validate parameter is not null or empty
            if(username == null || username == "")
            {
                throw new ArgumentException("Username not provided");
            }

            //create new domain user
            UserModel user = new UserModel();

            //pull user from db
            var dbuser = await _userRepository.GetUserByUserName(username);

            //validate db call returned user
            if(dbuser == null)
            {
                throw new Exception("User not found");
            }

            //convert db user to domain user
            user = EfUserMapper.DbEntityToCoreModel(dbuser);

            if (dbuser.PhotoId != null)
            {
                var photo = await _stockPhoto.GetPhotoById((long)dbuser.PhotoId);
                user.Photo = new DomainPhoto() { Id = photo.Id, URL = photo.Source.Medium };
            }

            return user;

        }

        /// <summary>
        /// Service method to pull users from database by User Id
        /// </summary>
        /// <param name="userIds">List of User Ids</param>
        /// <returns>List of Domain Model Users</returns>
        public async Task<List<UserModel>> GetUsersByUserId(List<long> userIds)
        {
            //create empty list of domain users 
            List<UserModel> allUsers = new List<UserModel>();

            //pull all users from db
            var dbUsers = await _userRepository.GetUsersByUserId(userIds);

            if(dbUsers != null) 
            {

                //map each db user to Domain Model and add to domain users list
                foreach (var dbUser in dbUsers)
                {
                    var user = EfUserMapper.DbEntityToCoreModel(dbUser);
                    if(dbUser.PhotoId != null)
                    {
                        var picture = await _stockPhoto.GetPhotoById((long)dbUser.PhotoId);
                        user.Photo = new DomainPhoto() { Id = picture.Id, URL = picture.Source.Medium };
                    }

                    allUsers.Add(user);
                }
            }
            else
            {
                throw new Exception("No Users found");
            }


            //return domain users list
            return allUsers;
        }

        /// <summary>
        /// Service method to log a user in
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>username</returns>
        public async Task<UserModel> Login(string userName, string password)
        {
            //validate inputs
            if(userName == null || password == null)
            {
                throw new Exception("Invalid input");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate user exists
            if(user == null)
            {
                throw new Exception("Invalid user");
            }

            //if user exists validate passwords match
            if(user.Password != password)
            {
                throw new Exception("Invalid password");
            }

            //if passwords match, update user status 
            await _userRepository.UpdateUserStatus(user.Id, true);

            //map db user to domain user
            var domainUser = EfUserMapper.DbEntityToCoreModel(user);

            if(user.PhotoId != null)
            {
                var picture = await _stockPhoto.GetPhotoById((long)user.PhotoId);
                domainUser.Photo = new DomainPhoto() { Id = picture.Id, URL = picture.Source.Medium};
            }

            return domainUser;
        }

        /// <summary>
        /// Service method to log a user out
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Completed task</returns>
        public async Task Logout(string userName)
        {
            //validate inputs
            if (userName == null)
            {
                throw new Exception("Invalid input");
            }

            //pull user account
            var user = await _userRepository.GetUserByUserName(userName);

            //validate user exists
            if (user == null)
            {
                throw new Exception("Invalid user");
            }

            //if user is valid
            await _userRepository.UpdateUserStatus(user.Id, false);

        }

        /// <summary>
        /// Service method to update individual user profile information
        /// </summary>
        /// <param name="updatedUser">User Model</param>
        /// <returns>Completed Task</returns>

        public async Task UpdateUserProfile(UserModel updatedUser)
        {
            //validate updatedUser
            var validUser = ValidateUser(updatedUser);

            //if updatedUser is valid
            if (validUser)
            {
                //pull user account
                var user = await _userRepository.GetUserByUserName(updatedUser.Username);

                //validate user
                if(user == null)
                {
                    throw new Exception("Invalid User");
                }

                //map updatedUser to user
                user = EfUserMapper.CoreModelToDbEntity(updatedUser);

                //submit updated information to repository
                await _userRepository.UpdateUserProfile(user);
                
            }
            else
            {
                throw new Exception("Invalid User");
            }

        }

        /// <summary>
        /// Private method to validate user properties
        /// </summary>
        /// <param name="coreUser">User Model</param>
        /// <returns>True if user is valid</returns>
        private bool ValidateUser(UserModel coreUser)
        {
            bool result;

            if (coreUser == null)
            {
                result = false;
            }
            else if(coreUser.FirstName == null)
            {
                result = false;
            }
            else if(coreUser.LastName == null)
            {
                result = false;
            }
            else if(coreUser.Password == null)
            {
                result = false;
            }
            else if(coreUser.Username == null)
            {
                result = false;
            }
            else
            {
                result = true;
            };

            return result;

        }
    }
}
