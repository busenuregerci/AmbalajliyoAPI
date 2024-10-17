using Ambalajliyo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AmbalajliyoRole> _roleManager;

        public RoleController(RoleManager<AmbalajliyoRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Yeni bir rol ekler.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new AmbalajliyoRole
                    {
                        Name = roleName
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return Ok(new { result = $"{roleName} rolü başarıyla oluşturuldu!" });
                    }
                    return BadRequest(result.Errors);
                }
                return BadRequest(new { error = $"{roleName} adlı rol zaten mevcuttur!" });
            }
            return BadRequest(new { error = "Geçerli bir rol adı girilmelidir!" });
        }

        /// <summary>
        /// Belirtilen id'ye ait rolü günceller.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newRoleName"></param>
        /// <returns></returns>
        [HttpPut("UpdateRole/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null && role.IsDeleted == false)
            {
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new { result = $"Seçilen rol adı {newRoleName} şeklinde güncellenmiştir!" });
                }
                return BadRequest(result.Errors);
            }
            return NotFound(new { error = "Girilen id'ye ait rol bulunamamıştır!" });
        }

        /// <summary>
        /// Belirtilen id'ye ait rolü siler.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null && role.IsDeleted == false)
            {
                role.IsDeleted = true;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new { result = $"Seçilen rol başarıyla silinmiştir!" });
                }
                return BadRequest(result.Errors);
            }
            return NotFound(new { error = "Girilen id'ye ail rol bulunamamıştır!" });
        }

        /// <summary>
        /// Bütün rolleri getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Where(r => !r.IsDeleted).ToList();
            return Ok(roles);
        }

        /// <summary>
        /// Belirtilen id'ye ait rolü getirir.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleById/{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null && role.IsDeleted == false)
            {
                return Ok(role);
            }
            return NotFound(new { error = "Girilen id'ye ait rol bulunamamıştır!" });
        }
    }
}