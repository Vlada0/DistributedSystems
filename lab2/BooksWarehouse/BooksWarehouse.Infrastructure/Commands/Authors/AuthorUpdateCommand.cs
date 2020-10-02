using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Authors
{
    public class AuthorUpdateCommand : ICommand
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Guid CountryId { get; set; }

        internal sealed class UpdateAuthorCommandHandler : ICommandHandler<AuthorUpdateCommand>
        {
            private readonly IMongoDbRepository<Author> _authors;
            private readonly IMapper _mapper;

            public UpdateAuthorCommandHandler(IMongoDbRepository<Author> authors, IMapper mapper)
            {
                _authors = authors;
                _mapper = mapper;
            }

            public async Task ExecuteAsync(AuthorUpdateCommand command)
            {
                var author = _mapper.Map<Author>(command);
                await _authors.ReplaceOneAsync(author);
            }
        }
    }
}