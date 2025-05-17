using BookLendingSystem.Application.Common;
using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Application.Interfaces.IServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookLendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }
        [Authorize]
             [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllBooksAsync());
        [Authorize(Roles = nameof(AuthConstants.AdminRole))]

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _service.GetBookByIdAsync(id));
        [Authorize(Roles = AuthConstants.AdminRole)]

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookDto dto)
        {
            await _service.AddBookAsync(dto);
            return Ok();
        }
        [Authorize(Roles = nameof(AuthConstants.AdminRole))]

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateBookDto dto)
        {
            await _service.UpdateBookAsync(id, dto);
            return Ok();
        }
        [Authorize(Roles = AuthConstants.AdminRole)]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBookAsync(id);
            return Ok();
        }
    }
}
