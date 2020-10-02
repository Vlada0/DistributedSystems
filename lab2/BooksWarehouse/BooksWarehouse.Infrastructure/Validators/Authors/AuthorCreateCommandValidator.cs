using BooksWarehouse.Infrastructure.Commands.Authors;
using FluentValidation;

namespace BooksWarehouse.Infrastructure.Validators.Authors
{
    public class AuthorCreateCommandValidator : AbstractValidator<AuthorCreateCommand>
    {
        public AuthorCreateCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty()
                .WithMessage($"{nameof(AuthorUpdateCommand.FirstName)} should not be empty.");
            RuleFor(c => c.LastName).NotEmpty()
                .WithMessage($"{nameof(AuthorUpdateCommand.LastName)} should not be empty.");
        }
    }
}