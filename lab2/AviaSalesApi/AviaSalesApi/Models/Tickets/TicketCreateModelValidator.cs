using FluentValidation;

namespace AviaSalesApi.Models.Tickets
{
    public class TicketCreateModelValidator : AbstractValidator<TicketCreateModel>
    {
        public TicketCreateModelValidator()
        {
            RuleFor(t => t.CountryFrom).NotEmpty()
                .WithMessage($"{nameof(TicketCreateModel.CountryFrom)} must be specified.");
            RuleFor(t => t.CityFrom).NotEmpty()
                .WithMessage($"{nameof(TicketCreateModel.CityFrom)} must be specified.");
            RuleFor(t => t.CountryTo).NotEmpty()
                .WithMessage($"{nameof(TicketCreateModel.CountryTo)} must be specified.");
            RuleFor(t => t.CityTo).NotEmpty()
                .WithMessage($"{nameof(TicketCreateModel.CityTo)} must be specified.");
        }
    }
}