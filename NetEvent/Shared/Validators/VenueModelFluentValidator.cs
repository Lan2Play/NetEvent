using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Shared.Validators
{
    public class VenueModelFluentValidator : AbstractValidator<VenueDto>
    {
        public VenueModelFluentValidator()
        {
#pragma warning disable S107
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.ZipCode)
                .NotEmpty();

            RuleFor(x => x.Number)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotEmpty();
#pragma warning restore S107
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<VenueDto>.CreateWithOptions((VenueDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
            {
                return Array.Empty<string>();
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
