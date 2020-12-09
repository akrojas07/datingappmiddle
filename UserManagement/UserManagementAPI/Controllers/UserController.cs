using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserManagement.Domain.Services.Interfaces;
using UserManagement.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Domain.Models;

namespace UserManagement.API.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private string _tokenstring;

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        /// <summary>
        /// Controller method to create new user
        /// </summary>
        /// <param name="newUser">NewUserRequest</param>
        /// <returns>Username as string</returns>
        [HttpPost]
        [Route("new")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewUser([FromBody] NewUserRequest newUser)
        {
            //validate entry
            var result = ValidateEntry(newUser);
            if (!result)
            {
                return StatusCode(400, "Invalid user entry");
            }

            try
            {
                //convert from request to user model 
                var modelUser = new UserModel()
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Username = newUser.Username,
                    Password = newUser.Password
                };

                //pass model user into service and generate JSON Token 
                var username = await _userService.CreateNewUser(modelUser);
                _tokenstring = GenerateJsonWebToken();

                return StatusCode(201, new { un = username, token = _tokenstring });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Controller method to delete existing user
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <param name="token">Token as string</param>
        /// <returns>Task completed</returns>
        [HttpDelete]
        [Route("delete/{username}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string username, string token)
        {
            //validate username is not empty
            if (username == null)
            {
                return StatusCode(400, "User not provided");
            }

            try
            {
                //call delete user account service
                await _userService.DeleteUserAccount(username);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /// <summary>
        /// Controller method to pull list of users by user id based on provided list
        /// </summary>
        /// <param name="userIds">List of long user ids</param>
        /// <returns>List of users</returns>

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersByUserId(List<long> userIds)
        {
            //validate empty list was not passed into method
            if(userIds.Count <= 0)
            {
                return StatusCode(400, "Users not provided");
            }

            try
            {
                //create a list to store users
                List<UserModel> users = new List<UserModel>();

                //call get method and store returned users in list
                users = await _userService.GetUsersByUserId(userIds);

                //validate list is not empty 
                if(users.Count <= 0)
                {
                    throw new Exception("Users not found");
                }

                //return status code 
                return StatusCode(200, users);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Controller method to log a user in 
        /// </summary>
        /// <param name="username">Username as a string</param>
        /// <param name="password">Password as a string</param>
        /// <returns>Username as a string</returns>

        [HttpPatch]
        [Route("login")]
        [Authorize]
        public async Task<IActionResult> Login([FromBody] string username, [FromBody] string password)
        {
            //validate inputs
            if(username == null || password == null)
            {
                return StatusCode(400, "Username or password not provided");
            }

            try
            {
                //call login service method 
                var user = await _userService.Login(username, password);

                //return username
                return StatusCode(200, user);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Controller method to log an existing user out
        /// </summary>
        /// <param name="username">Username as a string</param>
        /// <returns>Task Completed</returns>

        [HttpPatch]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] string username)
        {
            //validate username is not null
            if(username == null)
            {
                return StatusCode(400,"Username invalid");
            }

            try
            {
                //call logout service method
                await _userService.Logout(username);

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Controller method to update existing user profile
        /// </summary>
        /// <param name="updateUser">Update User request</param>
        /// <returns>Task Complete</returns>

        [HttpPut]
        [Route("update/profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest updateUser)
        {
            //validate user entry
            bool valid = ValidateEntry(updateUser);

            //if user is invalid, return bad request 
            if (!valid)
            {
                return StatusCode(400, "Bad Request");
            }

            try
            {
                //create new domain model and map request model
                UserModel domainModel = new UserModel()
                {
                    Username = updateUser.Username,
                    FirstName = updateUser.FirstName,
                    LastName = updateUser.LastName,
                    Password = updateUser.Password
                };

                //call update profile service method
                await _userService.UpdateUserProfile(domainModel);

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Private controller method to validate user request entries
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns>True is user request entry is valid</returns>

        private bool ValidateEntry(BaseUserRequest baseRequest)
        {
            if (baseRequest == null)
            {
                return false;
            }
            else if (baseRequest.FirstName == null)
            {
                return false;
            }
            else if (baseRequest.LastName == null)
            {
                return false;
            }
            else if (baseRequest.Password == null)
            {
                return false;
            }
            else if (baseRequest.Username == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Private controller method to create a JSON token
        /// </summary>
        /// <returns>New JSON token as string</returns>
        private string GenerateJsonWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config.GetSection("Jwt:Issuer").Value,
                    _config.GetSection("Jwt:Issuer").Value,
                    null,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
