using System.Security.Claims;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using Riok.Mapperly.Abstractions;

namespace NetEvent.Shared
{
    [Mapper]
    public partial class DtoMapper
    {
        public static DtoMapper Mapper { get; } = new DtoMapper();

        public partial UserDto ApplicaitonUserToUserDto(ApplicationUser applicationUser);

        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.IsAuthenticated)}", nameof(CurrentUserDto.IsAuthenticated))]
        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.Name)}", nameof(CurrentUserDto.UserName))]
        [MapperIgnore(nameof(CurrentUserDto.Claims))]
        public partial CurrentUserDto ClaimsPrincipalToCurrentUserDto(ClaimsPrincipal claimsPrincipal);

        public partial OrganizationData OrganizationDataDtoToOrganizationData(OrganizationDataDto organizationData);

        public partial OrganizationDataDto OrganizationDataToOrganizationDataDto(OrganizationData organizationData);
    }
}
