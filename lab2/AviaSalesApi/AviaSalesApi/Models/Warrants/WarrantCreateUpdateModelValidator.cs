using FluentValidation;

namespace AviaSalesApi.Models.Warrants
{
    public class WarrantCreateUpdateModelValidator : AbstractValidator<WarrantCreateUpdateModel>
    {
        public WarrantCreateUpdateModelValidator()
        {
            RuleFor(w => w.PassengerIban)
                .MinimumLength(13)
                .MaximumLength(20)
                .WithMessage("Iban should be not less than 13 and not greater than 20 characters length");
            RuleFor(w => w.PassportId)
                .NotNull()
                .WithMessage("Passport number should be provided.");
        }
    }
}