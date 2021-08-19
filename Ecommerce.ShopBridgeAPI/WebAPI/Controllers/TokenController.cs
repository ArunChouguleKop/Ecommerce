using BusinessPersister.BuisnessObject;
using Ecommerce.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopBridge.WebAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        private UserPersister _userPersister;
        public IConfiguration _configuration;
        public TokenController(UserPersister userPersister, IConfiguration iConfig)
        {
            this._userPersister = userPersister;
            _configuration = iConfig;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetToken(User _userData)
        {

            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user  = _userPersister.Login(_userData);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString())
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
                    user.Token = returnToken;
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
