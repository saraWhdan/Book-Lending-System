using BookLendingSystem.Domain.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Interfaces.IRepositories
{
    public interface IBorrowedBookRepository
    {
        Task<BorrowedBook> GetBorrowedBookByUserAsync(string memberId);
        Task AddAsync(BorrowedBook borrowed);
        Task<BorrowedBook> GetByIdAsync(int id);
        Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync(DateTime today);
        Task SaveAsync();
    }
}
