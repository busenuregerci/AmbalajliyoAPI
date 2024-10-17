using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides functionality to generate JWT tokens for users.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AmbalajliyoRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration settings for JWT tokens.</param>
        /// <param name="roleManager">Role manager for retrieving role information.</param>
        public TokenService(IConfiguration configuration, RoleManager<AmbalajliyoRole> roleManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing user information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the JWT token as a string.</returns>
        public async Task<string> GenerateToken(UserDto userDto)
        {
            string issuer = _configuration["JwtTokenSettings:Issuer"];
            string audience = _configuration["JwtTokenSettings:Audience"];
            string key = _configuration["JwtTokenSettings:Key"];
            string lifetime = _configuration["JwtTokenSettings:Lifetime"];

            DateTime expiration = DateTime.Now.AddMinutes(Convert.ToInt32(lifetime));

            var role = await _roleManager.FindByIdAsync(userDto.AmbalajliyoRoleId);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim(ClaimTypes.Surname, userDto.Surname),
                new Claim(ClaimTypes.NameIdentifier, userDto.UserName),
                new Claim(ClaimTypes.Role, role.Name),
                new Claim("userId", userDto.Id.ToString()),
                new Claim("isDeleted", userDto.IsDeleted.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
