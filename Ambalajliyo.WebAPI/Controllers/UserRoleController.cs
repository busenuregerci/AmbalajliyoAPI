using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserManager<AmbalajliyoUser> _userManager;
        private readonly RoleManager<AmbalajliyoRole> _roleManager;

        public UserRoleController(UserManager<AmbalajliyoUser> userManager, RoleManager<AmbalajliyoRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("GetAllUsersWithRoles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    AmbalajliyoRoleId = roleName != null ? (await _roleManager.FindByNameAsync(roleName)).Id : null,
                    //RoleName = roleName
                });
            }
            return Ok(userDtos);
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(userDto.AmbalajliyoRoleId);
            if (role == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return Ok();
        }

        [HttpDelete("RemoveRoleFromUser/{userId}")]
        public async Task<IActionResult> RemoveRoleFromUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            return Ok();
        }

        [HttpPut("UpdateUserRole/{userId}")]
        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            var newRole = await _roleManager.FindByIdAsync(roleId);
            if (newRole == null)
            {
                return NotFound("Role not found");
            }

            await _userManager.AddToRoleAsync(user, newRole.Name);

            // Update the AmbalajliyoRoleId in the AspNetUsers table
            user.AmbalajliyoRoleId = roleId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update the user role ID");
            }
            return Ok();
        }
    }
}