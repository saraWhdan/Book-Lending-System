using AutoMapper;
using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Domain.Entities.Business;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowedBookRepository _borrowRepo;
        private readonly IBaseRepository<Book> _bookRepo;
        private readonly IMapper _mapper;
     

        public BorrowService(IBorrowedBookRepository borrowRepo, IBaseRepository<Book> bookRepo, IMapper mapper)
        {
            _borrowRepo = borrowRepo;
            _bookRepo = bookRepo;
            _mapper = mapper;
          
        }

        public async Task BorrowBookAsync(string memberId, int bookId)
        {
            var book = await _bookRepo.GetById(bookId);
            if (book == null || book.Quantity <= 0)
                throw new Exception("Book not available");

            var borrowed = new BorrowedBook
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.UtcNow,
                ReturnDate = null
            };

            book.Quantity--;
            await _borrowRepo.AddAsync(borrowed);
            await _bookRepo.Update(book);
        }

        public async Task ReturnBookAsync(int borrowId)
        {
            var borrowed = await _borrowRepo.GetByIdAsync(borrowId);
            if (borrowed == null )
                throw new Exception("Invalid return");

            borrowed.ReturnDate = DateTime.UtcNow;
            var book = await _bookRepo.GetById(borrowed.BookId.Value);
            book.Quantity++;
          
            await _bookRepo.Update(book);
        }

        public async Task<IEnumerable<BorrowedBook>> GetOverdueBooksAsync() => await _borrowRepo.GetOverdueBooksAsync(DateTime.UtcNow);
    }
}
