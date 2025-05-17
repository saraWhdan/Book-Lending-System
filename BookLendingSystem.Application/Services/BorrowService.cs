using AutoMapper;
using BookLendingSystem.Application.Dtos;
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
        private readonly IBorrowedBookRepository _borrowedBookRepo;
        private readonly IMapper _mapper;

        public BorrowService(IBorrowedBookRepository borrowedBookRepo, IMapper mapper)
        {
            _borrowedBookRepo = borrowedBookRepo;
            _mapper = mapper;
        }

        public async Task<BorrowedBookDto> BorrowBookAsync(BorrowBookDto dto, string userId)
        {
            var currentBorrowed = await _borrowedBookRepo.GetBorrowedBookByUserAsync(userId);
            if (currentBorrowed != null)
                throw new InvalidOperationException("User has an active borrowed book");

            var borrowed = new BorrowedBook
            {
                BookId = dto.BookId,
                MemberId = userId,
                BorrowDate = DateTime.UtcNow
            };

            await _borrowedBookRepo.AddAsync(borrowed);
            await _borrowedBookRepo.SaveAsync();

            return _mapper.Map<BorrowedBookDto>(borrowed);
        }

        public async Task<BorrowedBookDto> GetCurrentBorrowedBookAsync(string userId)
        {
            var borrowed = await _borrowedBookRepo.GetBorrowedBookByUserAsync(userId);
            if (borrowed == null)
                throw new InvalidOperationException("No active borrowed book found");

            return _mapper.Map<BorrowedBookDto>(borrowed);
        }

        public async Task<IEnumerable<BorrowedBookDto>> GetOverdueBooksAsync()
        {
            var overdue = await _borrowedBookRepo.GetOverdueBooksAsync(DateTime.UtcNow);
            return _mapper.Map<IEnumerable<BorrowedBookDto>>(overdue);
        }

        public async Task<BorrowedBookDto> ReturnBookAsync(string userId)
        {
            var borrowed = await _borrowedBookRepo.GetBorrowedBookByUserAsync(userId);
            if (borrowed == null)
                throw new InvalidOperationException("No active borrowed book to return.");

            borrowed.ReturnDate = DateTime.UtcNow;

            await _borrowedBookRepo.SaveAsync();
            return _mapper.Map<BorrowedBookDto>(borrowed);
        }
    }
}
