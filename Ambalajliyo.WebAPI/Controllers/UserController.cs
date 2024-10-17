using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.DAL.Entities;
using Ambalajliyo.WebAPI.ActionFilters;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ambalajliyo.BLL.Interfaces;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AmbalajliyoUser> _userManager;
        private readonly ILogService _logService;
        private readonly ITokenService _tokenService;

        public UserController(UserManager<AmbalajliyoUser> userManager, ILogService logService, ITokenService tokenService)
        {
            _userManager = userManager;
            _logService = logService;
            _tokenService = tokenService;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Where(u => !u.IsDeleted).ToList();
            var userDtos = users.Adapt<List<UserDto>>();
            return Ok(userDtos);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "Kullanıcı bulunamadı!" });
            }
            user.IsDeleted = true;
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { result = $"{user.UserName} kullanıcısı başarıyla silindi!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("RemoveUser/{id}")]
        public async Task<IActionResult> RemoveUser(string id, [FromBody] UserDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "Kullanıcı bulunamadı!" });
            }
            user.IsDeleted = true;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { result = $"{model.UserName} kullanıcısı başarıyla güncellendi!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "Kullanıcı bulunamadı!" });
            }

            var userDto = user.Adapt<UserDto>();
            return Ok(userDto);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = model.Adapt<AmbalajliyoUser>();
                var hasher = new PasswordHasher<AmbalajliyoUser>();
                user.PasswordHash = hasher.HashPassword(user, model.PasswordHash);
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { result = $"{model.UserName} kullanıcısı başarıyla oluşturuldu!" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(new { error = "Geçerli bir kullanıcı adı, email, isim, soyisim ve şifre girilmelidir!" });
        }

        [LoginAttemptFilter]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Email ve şifre gereklidir.");
            }

            // Kullanıcıyı email ile bul
            var kullanici = await _userManager.Users.FirstOrDefaultAsync(a => a.Email == loginDto.Email);
            if (kullanici == null)
            {
                return Unauthorized(new { mesaj = "Geçersiz kullanıcı adı veya şifre" });
            }

            // Şifreyi doğrula
            var hasher = new PasswordHasher<AmbalajliyoUser>();
            var result = hasher.VerifyHashedPassword(kullanici, kullanici.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { mesaj = "Geçersiz email veya şifre" });
            }

            var kullaniciDto = kullanici.Adapt<UserDto>();

            // Token oluştur
            var token = await _tokenService.GenerateToken(kullaniciDto);

            // Token ve kullanıcı bilgilerini dön
            return Ok(token);
        }

        [HttpGet("GetAllLog")]
        public async Task<IActionResult> GetAllLog(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var logs = await _logService.GetAllLogs(startDate, endDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existUser = await _userManager.FindByIdAsync(userId.ToString());
                if (existUser == null)
                {
                    return NotFound($"Kullanıcı bulunamadı: {userId}");
                }

                // Kullanıcı bilgilerini güncelle
                existUser.UserName = userDto.UserName;
                existUser.Email = userDto.Email;
                existUser.Name = userDto.Name;
                existUser.Surname = userDto.Surname;
                existUser.AmbalajliyoRoleId = userDto.AmbalajliyoRoleId;
                // Diğer gerekli kullanıcı bilgilerini güncelle

                // Şifre hash'ini güncelle
                if (!string.IsNullOrEmpty(userDto.PasswordHash))
                {
                    existUser.PasswordHash = userDto.PasswordHash;
                }

                var updateResult = await _userManager.UpdateAsync(existUser);
                if (!updateResult.Succeeded)
                {
                    return BadRequest(updateResult.Errors);
                }

                return Ok(existUser);
            }
            catch (Exception ex)
            {
                // Hata loglama yapılabilir
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
        
        [HttpGet("VerifyPassword/{id}/{sifre}")]
        public async Task<IActionResult> VerifyPassword(string id, string sifre)
        {
            // Kullanıcıyı bul
            var kullanici = await _userManager.FindByIdAsync(id);

            if (kullanici == null)
            {
                return Unauthorized(new { mesaj = "Kullanıcı bulunamadı" });
            }

            // Şifreyi doğrula
            var hasher = new PasswordHasher<AmbalajliyoUser>();
            var result = hasher.VerifyHashedPassword(kullanici, kullanici.PasswordHash, sifre);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { mesaj = "Geçersiz şifre" });
            }

            return Ok(new { mesaj = "Şifre doğrulandı", result });
        }

        [HttpGet("HashPassword/{newPassword}")]
        public async Task<IActionResult> HashPassword(string newPassword)
        {
            // Initialize the PasswordHasher
            var passwordHasher = new PasswordHasher<object>();

            // Create a dummy user object for hashing (the user object isn't used in password calculation)
            object dummyUser = new object();

            // Hash the password
            string hashedPassword = passwordHasher.HashPassword(dummyUser, newPassword);

            // Return the hashed password as a JSON response
            return Ok(new { hashedPassword });
        }
    }
}