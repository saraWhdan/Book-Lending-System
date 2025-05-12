using BookLendingSystem.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Interfaces.IServices
{
    public interface IBookService
    {

        Task<BookDto> GetBookByIdAsync(int id);
            Task<IEnumerable<BookDto>> GetAllBooksAsync();
            Task AddBookAsync(CreateBookDto dto);
            Task UpdateBookAsync(int id, CreateBookDto dto);
            Task DeleteBookAsync(int id);
        

    }
}
