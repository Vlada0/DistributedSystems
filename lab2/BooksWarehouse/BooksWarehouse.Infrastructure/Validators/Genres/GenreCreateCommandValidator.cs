using BooksWarehouse.Infrastructure.Commands.Genres;
using FluentValidation;

namespace BooksWarehouse.Infrastructure.Validators.Genres
{
    public class GenreCreateCommandValidator : AbstractValidator<GenreCreateCommand>
    {
        public GenreCreateCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty()
                .WithMessage($"{nameof(GenreCreateCommand.Name)} must be specified.");
        }
    }
}