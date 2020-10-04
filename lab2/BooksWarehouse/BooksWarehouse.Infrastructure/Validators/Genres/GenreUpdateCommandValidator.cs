using BooksWarehouse.Infrastructure.Commands.Genres;
using FluentValidation;

namespace BooksWarehouse.Infrastructure.Validators.Genres
{
    public class GenreUpdateCommandValidator : AbstractValidator<GenreUpdateCommand>
    {
        public GenreUpdateCommandValidator()
        {
            RuleFor(g => g.Id).NotNull()
                .WithMessage($"{nameof(GenreCreateCommand.Id)} must be specified.");
            RuleFor(g => g.Name).NotEmpty()
                .WithMessage($"{nameof(GenreCreateCommand.Name)} must be specified.");
        }
    }
}