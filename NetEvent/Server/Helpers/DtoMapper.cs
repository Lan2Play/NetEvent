using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using Riok.Mapperly.Abstractions;
using System.Security.Claims;

namespace NetEvent.Shared
{
    [Mapper]
    public partial class DtoMapper
    {
        public static DtoMapper Mapper { get; } = new DtoMapper();

        public partial User ApplicaitonUserToUser(ApplicationUser applicationUser);

        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.IsAuthenticated)}", nameof(CurrentUser.IsAuthenticated))]
        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.Name)}", nameof(CurrentUser.UserName))]
        [MapperIgnore(nameof(CurrentUser.Claims))]
        public partial CurrentUser ClaimsPrincipalToCurrentUser(ClaimsPrincipal claimsPrincipal);

        public partial Server.Models.OrganizationData DtoOrganizationDataToOrganizationData(Dto.OrganizationData organizationData);

        public partial Dto.OrganizationData OrganizationDataToDtoOrganizationData(Server.Models.OrganizationData organizationData);
    }
}
