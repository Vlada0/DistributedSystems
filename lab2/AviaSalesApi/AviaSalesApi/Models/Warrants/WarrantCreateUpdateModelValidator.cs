using FluentValidation;

namespace AviaSalesApi.Models.Warrants
{
    public class WarrantCreateUpdateModelValidator : AbstractValidator<WarrantCreateUpdateModel>
    {
        public WarrantCreateUpdateModelValidator()
        {
            RuleFor(w => w.PassengerIban)
                .Length(13, 13).WithMessage("Iban should be 13 characters length");
        }
    }
}