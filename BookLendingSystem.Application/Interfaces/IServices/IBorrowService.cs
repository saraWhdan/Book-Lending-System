using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Domain.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Interfaces.IServices
{
    public interface IBorrowService
    {
        Task<BorrowedBookDto> BorrowBookAsync(BorrowBookDto dto, string userId);
        Task<BorrowedBookDto> GetCurrentBorrowedBookAsync(string userId);
        Task<IEnumerable<BorrowedBookDto>> GetOverdueBooksAsync();
        Task<BorrowedBookDto> ReturnBookAsync(string userId);
    }

}
