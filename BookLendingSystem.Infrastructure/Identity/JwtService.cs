using BookLendingSystem.Application.Helpers;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Infrastructure.Identity
{
    public class JwtService : IAuthService
    {
        private readonly JWT _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IOptions<JWT> options, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = options.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {// check the roles 
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                claims: claims,
                signingCredentials: creds

               
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       

        public Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                IsRevoked = false
            };
            return Task.FromResult(refreshToken);
        }
    }

}
