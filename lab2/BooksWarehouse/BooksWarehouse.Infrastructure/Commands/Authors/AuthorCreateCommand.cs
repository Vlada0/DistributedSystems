using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Authors
{
    public class AuthorCreateCommand : ICommand
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Guid CountryId { get; set; }
        
        internal sealed class AuthorCreateCommandHandler : ICommandHandler<AuthorCreateCommand>
        {
            private readonly IMongoDbRepository<Author> _authors;
            private readonly IMapper _mapper;
            
            public AuthorCreateCommandHandler(IMongoDbRepository<Author> authors, IMapper mapper)
            {
                _authors = authors;
                _mapper = mapper;
            }

            public async Task ExecuteAsync(AuthorCreateCommand command)
            {
                var authorDocument = _mapper.Map<Author>(command);
                command.Id = await _authors.InsertOneAsync(authorDocument);
            }
        }
    }
}