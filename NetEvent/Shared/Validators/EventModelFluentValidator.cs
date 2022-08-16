using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Shared.Validators
{
    public class EventModelFluentValidator : AbstractValidator<EventDto>
    {
        public EventModelFluentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotNull();

            RuleFor(x => x.EndDate)
                .NotNull()
                .GreaterThan(x => x.StartDate);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<EventDto>.CreateWithOptions((EventDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
            {
                return Array.Empty<string>();
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };

        //public Func<EventDto, Task<IEnumerable<string>>> Validation => InternalValidateAsync;

        //private async Task<IEnumerable<string>> InternalValidateAsync(EventDto eventToValidate)
        //{
        //    var validationResult = await ValidateAsync(eventToValidate);
        //    return validationResult.Errors.Select(x => x.ErrorMessage);
        //}
    }
}
