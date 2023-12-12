using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Shared.Validators
{
    public class EventTicketTypeModelFluentValidator : AbstractValidator<EventTicketTypeDto>
    {
        private const int _MaxNameLength = 100;

        public EventTicketTypeModelFluentValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .Length(1, _MaxNameLength);

            RuleFor(x => x.Price)
               .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AvailableTickets)
               .GreaterThan(0);

            RuleFor(x => x.SellEndDate)
               .GreaterThan(x => x.SellStartDate);

            RuleFor(x => x.SellStartDate)
               .NotNull();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<EventTicketTypeDto>.CreateWithOptions((EventTicketTypeDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
            {
                return Array.Empty<string>();
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
