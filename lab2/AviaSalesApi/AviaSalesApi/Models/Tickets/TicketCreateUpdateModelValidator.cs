using FluentValidation;

namespace AviaSalesApi.Models.Tickets
{
    public class TicketCreateUpdateModelValidator : AbstractValidator<TicketCreateUpdateModel>
    {
        public TicketCreateUpdateModelValidator()
        {
            RuleFor(t => t.CountryFrom).NotEmpty()
                .WithMessage($"{nameof(TicketCreateUpdateModel.CountryFrom)} must be specified.");
            RuleFor(t => t.CityFrom).NotEmpty()
                .WithMessage($"{nameof(TicketCreateUpdateModel.CityFrom)} must be specified.");
            RuleFor(t => t.CountryTo).NotEmpty()
                .WithMessage($"{nameof(TicketCreateUpdateModel.CountryTo)} must be specified.");
            RuleFor(t => t.CityTo).NotEmpty()
                .WithMessage($"{nameof(TicketCreateUpdateModel.CityTo)} must be specified.");
        }
    }
}