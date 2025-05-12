using AutoMapper;
using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Domain.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBaseRepository<Book> _bookRepo;
        private readonly IMapper _mapper;

        public BookService(IBaseRepository<Book> bookRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetAll();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _bookRepo.GetById(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task AddBookAsync(CreateBookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _bookRepo.Create(book);
        }

        public async Task UpdateBookAsync(int id, CreateBookDto dto)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null) throw new Exception("Book not found");
            _mapper.Map(dto, book);
            await _bookRepo.Update(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null) throw new Exception("Book not found");
            await _bookRepo.Delete(book);
        }
    }
}
