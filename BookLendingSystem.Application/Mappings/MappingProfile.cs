using AutoMapper;
using BookLendingSystem.Application.Dtos;
using BookLendingSystem.Domain.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, Book>().ReverseMap();
        }
    }
}
