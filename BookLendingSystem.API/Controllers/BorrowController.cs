using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Domain.Entities.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BorrowController(IBorrowService borrowService, UserManager<ApplicationUser> userManager)
        {
            _borrowService = borrowService;
            _userManager = userManager;
        }

        [HttpPost("borrow")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDto dto)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _borrowService.BorrowBookAsync(dto, userId);
            return Ok(result);
        }

        [HttpGet("current")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetCurrentBorrowedBook()
        {
            var userId = _userManager.GetUserId(User);
            var result = await _borrowService.GetCurrentBorrowedBookAsync(userId);

            return Ok(result);
        }

        [HttpGet("overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOverdueBooks()
        {
            var result = await _borrowService.GetOverdueBooksAsync();
            return Ok(result);
        }
        [HttpPost("return")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> ReturnBook()
        {
            var userId = _userManager.GetUserId(User);
            var result = await _borrowService.ReturnBookAsync(userId);
            return Ok(result);
        }
    }
}
