using Ambalajliyo.BLL.DTOs;
using System;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines the contract for token-related operations in the business logic layer.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for a specified user.
        /// </summary>
        /// <param name="userDto">The user data for which the token is to be generated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token as a string.</returns>
        Task<string> GenerateToken(UserDto userDto);
    }
}
