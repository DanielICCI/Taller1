using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Taller1.DTOs.Auth;
using Taller1.Models;

namespace Taller1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
public async Task<IActionResult> Register(RegisterDTO dto)
{
    var user = new ApplicationUser
    {
        UserName = dto.Email,
        Email = dto.Email,
        FullName = dto.FullName,
        DateOfBirth = dto.Birthday,
        Description = dto.Description
    };

    var result = await _userManager.CreateAsync(user, dto.Password);

    if (!result.Succeeded)
        return BadRequest(result.Errors);

    //se asigna el rol automáticamente
    await _userManager.AddToRoleAsync(user, "Cliente");

    return Ok("Usuario registrado correctamente.");
}
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Credenciales inválidas");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Credenciales inválidas");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("FullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}