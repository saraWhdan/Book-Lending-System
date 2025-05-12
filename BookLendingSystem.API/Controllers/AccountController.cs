using BookLendingSystem.Application.Common;
using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public static List<RefreshToken> RefreshTokens = new();

        public AccountController(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, AuthConstants.MemberRole);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var accessToken = await _authService.GenerateAccessTokenAsync(user);
            var refreshToken = await _authService.GenerateRefreshTokenAsync(user.Id);
            RefreshTokens.Add(refreshToken);

            return Ok(new { token = accessToken, refreshToken = refreshToken.Token });
        }

     
    }
}
