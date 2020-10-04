using BooksWarehouse.Infrastructure.Commands.Genres;
using FluentValidation;

namespace BooksWarehouse.Infrastructure.Validators.Genres
{
    public class GenreDeleteByNameCommandValidator : AbstractValidator<GenreDeleteByNameCommand>
    {
        public GenreDeleteByNameCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty()
                .WithMessage($"Name must be specified.");
        }
    }
}