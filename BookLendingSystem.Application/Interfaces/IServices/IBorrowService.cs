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
        Task BorrowBookAsync(string memberId, int bookId);
        Task ReturnBookAsync(int borrowId);
        Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync();
    }

}
