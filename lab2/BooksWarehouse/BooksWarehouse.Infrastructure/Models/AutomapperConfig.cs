using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Commands.Authors;
using BooksWarehouse.Infrastructure.Models.Authors;
using BooksWarehouse.Infrastructure.Models.Countries;

namespace BooksWarehouse.Infrastructure.Models
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<AuthorCreateCommand, Author>()
                .ForMember(a => a.Id, opt => opt.Ignore());

            CreateMap<AuthorUpdateCommand, Author>();

            CreateMap<Country, CountryModel>();
        }
    }
}