using AutoMapper;
using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Application.Interfaces.IRepositories;
using BookLendingSystem.Application.Interfaces.IServices;
using BookLendingSystem.Application.Mappings;
using BookLendingSystem.Application.Services;
using BookLendingSystem.Domain.Entities.Business;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Test.Application
{
    public class BookTest
    {
        private readonly Mock<IBaseRepository<Book>> bookRepo;
        private readonly BookService BookService;
        private readonly IMapper _mapper;
        public BookTest(  )
        {
            bookRepo = new Mock<IBaseRepository<Book>>();
            var config = new MapperConfiguration(cfg =>
            {
                // Register all your profiles here
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        
       
            BookService = new BookService(bookRepo.Object, _mapper);
        }
        [Fact]
        public async Task AddAsync()
        {
            var createBookDto = new CreateBookDto
            {
                Title = "Happy",
                Quantity = 2,
                Author= "Author test",
                ISBN= "132436657" ,
                PublishedDate = DateTime.Now,
               

                
            };
            var book = new Book
            {
                Title = "Happy",
                Quantity = 2,
                Author = "Author test",
                ISBN = "132436657",
                PublishedDate = DateTime.Now,


            };
            var returnCreateBook = new Book
            {
                Title = "Happy",
                Quantity = 2,
                Author = "Author test",
                ISBN = "132436657",
                PublishedDate = DateTime.Now,
            };
            bookRepo.Setup(x => x.Create(It.IsAny<Book>()))
                .ReturnsAsync(returnCreateBook);
            //action
            var result = await BookService.AddBookAsync(createBookDto);
            Assert.NotNull(result);
            Assert.IsType<Book>(result);
            Assert.Equal(returnCreateBook.Title, result.Title);
            Assert.Equal(returnCreateBook.Quantity, result.Quantity);
           




        }
    }
}
