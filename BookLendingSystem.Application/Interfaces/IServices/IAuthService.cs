using BookLendingSystem.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
    }
}
