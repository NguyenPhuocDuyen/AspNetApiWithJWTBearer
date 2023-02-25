using Asp_Net_API_Authenticatoin_With_Bearer_JWT.Models;
using Asp_Net_API_Authenticatoin_With_Bearer_JWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace Asp_Net_API_Authenticatoin_With_Bearer_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UsersController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            //check user exist
            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            //tao handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //encoding key in json
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //set description for token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //create token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //convert token to string
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        // Other API endpoints with not JWT authentication
        [HttpGet("GetUsersNotToken")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        // Other API endpoints with JWT authentication
        [Authorize]
        [HttpGet("GetUsersWithToken")]
        public ActionResult<IEnumerable<User>> GetUserWithToken()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }
    }
}
