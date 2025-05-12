using BookLendingSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _service;

        public BorrowController(IBorrowService service)
        {
            _service = service;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromQuery] string memberId, [FromQuery] int bookId)
        {
            await _service.BorrowBookAsync(memberId, bookId);
            return Ok();
        }

        [HttpPost("return/{borrowId}")]
        public async Task<IActionResult> Return(int borrowId)
        {
            await _service.ReturnBookAsync(borrowId);
            return Ok();
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> Overdue()
        {
            var result = await _service.GetOverdueBooksAsync();
            return Ok(result);
        }
    }
}
