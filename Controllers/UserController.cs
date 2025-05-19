using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller1.Models;
using Taller1.DTOs.User;

namespace Taller1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Obtener los datos del usuario autenticado
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var userInfo = new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.DateOfBirth
            };

            return Ok(userInfo);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateUserDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            user.FullName = dto.FullName;
            user.Description = dto.Description;
            user.DateOfBirth = dto.DateOfBirth;

             var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

        return Ok("Perfil actualizado correctamente.");
        }
    }
}