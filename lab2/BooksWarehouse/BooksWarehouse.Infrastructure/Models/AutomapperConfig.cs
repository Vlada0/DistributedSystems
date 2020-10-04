using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Commands.Authors;
using BooksWarehouse.Infrastructure.Commands.Genres;
using BooksWarehouse.Infrastructure.Models.Authors;
using BooksWarehouse.Infrastructure.Models.Genres;

namespace BooksWarehouse.Infrastructure.Models
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Author, AuthorModel>();
            CreateMap<AuthorCreateCommand, Author>()
                .ForMember(a => a.Id, opt => opt.Ignore());
            CreateMap<AuthorUpdateCommand, Author>();

            CreateMap<Genre, GenreModel>()
                .ForMember(g => g.GenreId, opt => 
                    opt.MapFrom(src => src.Id));
            CreateMap<GenreCreateCommand, Genre>()
                .ForMember(g => g.Id, opt => opt.Ignore());
            CreateMap<GenreUpdateCommand, Genre>();
        }
    }
}