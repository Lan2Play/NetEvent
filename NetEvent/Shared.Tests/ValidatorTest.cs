using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NetEvent.Shared.Dto.Event;
using NetEvent.Shared.Validators;
using NetEvent.TestHelper;
using Xunit;

namespace NetEvent.Shared.Tests
{
    [ExcludeFromCodeCoverage]
    public class ValidatorTest
    {
        [Fact]
        public async Task ValidatorTest_VenueModelFluentValidator_Test()
        {
            var fake = Fakers.VenueFaker().Generate().ToVenueDto();

            var validator = new VenueModelFluentValidator();
            var validationResult = await validator.ValidateAsync(fake);
            var errorList = await validator.ValidateValue(fake, nameof(VenueDto.Name));
            Assert.True(validationResult.IsValid);
            Assert.Empty(errorList);

            // Missing Name
            fake.Name = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.Name));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Long Name
            fake.Name = string.Join("", Enumerable.Repeat("A", 101));
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.Name));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing Zipcode
            fake = Fakers.VenueFaker().Generate().ToVenueDto();
            fake.ZipCode = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.ZipCode));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing Number
            fake = Fakers.VenueFaker().Generate().ToVenueDto();
            fake.Number = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.Number));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing City
            fake = Fakers.VenueFaker().Generate().ToVenueDto();
            fake.City = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.City));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing Street
            fake = Fakers.VenueFaker().Generate().ToVenueDto();
            fake.Street = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(VenueDto.Street));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);
        }

        [Fact]
        public async Task ValidatorTest_EventModelFluentValidator_Test()
        {
            var fake = Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate().ToEventDto();

            var validator = new EventModelFluentValidator();
            var validationResult = await validator.ValidateAsync(fake);
            var errorList = await validator.ValidateValue(fake, nameof(EventDto.Name));
            Assert.True(validationResult.IsValid);
            Assert.Empty(errorList);

            // Missing Name
            fake.Name = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(EventDto.Name));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Long Name
            fake.Name = string.Join("", Enumerable.Repeat("A", 101));
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(EventDto.Name));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing Description
            fake = Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate().ToEventDto();
            fake.Description = string.Empty;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(EventDto.Description));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing StartDate
            fake = Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate().ToEventDto();
            fake.StartDate = null;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(EventDto.StartDate));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);

            // Missing EndDate
            fake = Fakers.EventFaker(Fakers.VenueFaker().Generate(2)).Generate().ToEventDto();
            fake.EndDate = null;
            validationResult = await validator.ValidateAsync(fake);
            errorList = await validator.ValidateValue(fake, nameof(EventDto.EndDate));
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(errorList);
        }
    }
}
