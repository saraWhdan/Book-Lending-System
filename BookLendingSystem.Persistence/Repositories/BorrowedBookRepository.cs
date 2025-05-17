using BookLendingSystem.Application.Common;
using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Domain.Entities.Business;
using BookLendingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Persistence.Repositories
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly BorrowSettings _borrowSettings;
        public BorrowedBookRepository(ApplicationDbContext context, IOptions<BorrowSettings> options)
        {
            _context = context;
            _borrowSettings = options.Value;
        }
        public async Task<BorrowedBook> GetBorrowedBookByUserAsync(string memberId) =>
              await _context.BorrowedBooks.FirstOrDefaultAsync(b => b.MemberId == memberId && b.ReturnDate == null);

        public async Task AddAsync(BorrowedBook borrowed)
        {
            await _context.BorrowedBooks.AddAsync(borrowed);
        }

        public async Task<BorrowedBook> GetByIdAsync(int id)
        {
            var data = await _context.BorrowedBooks.FindAsync(id);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync(DateTime today)
        {
            return await _context.BorrowedBooks
                .Where(b => b.ReturnDate == null && b.BorrowDate.AddDays(_borrowSettings.BorrowPeriodInDays) < today)
                .ToListAsync();
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }

}
