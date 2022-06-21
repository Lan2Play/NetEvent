using System.Security.Claims;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;
using Riok.Mapperly.Abstractions;

namespace NetEvent.Shared
{
    [Mapper]
    public partial class DtoMapper
    {
        public static DtoMapper Mapper { get; } = new DtoMapper();

        public partial UserDto ApplicaitonUserToUserDto(ApplicationUser applicationUser);

        [MapperIgnore(nameof(CurrentUserDto.ProfileImage))]
        [MapperIgnore(nameof(AdminUserDto.Role))]
        public partial AdminUserDto ApplicaitonUserToAdminUserDto(ApplicationUser applicationUser);

        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.IsAuthenticated)}", nameof(CurrentUserDto.IsAuthenticated))]
        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.Name)}", nameof(CurrentUserDto.UserName))]
        [MapperIgnore(nameof(CurrentUserDto.Claims))]
        public partial CurrentUserDto ClaimsPrincipalToCurrentUserDto(ClaimsPrincipal claimsPrincipal);

        [MapperIgnore(nameof(RoleDto.Claims))]
        public partial RoleDto ApplicationRoleToRoleDto(ApplicationRole applicationRole);

        [MapperIgnore(nameof(ApplicationRole.NormalizedName))]
        [MapperIgnore(nameof(ApplicationRole.ConcurrencyStamp))]
        public partial ApplicationRole RoleDtoToApplicationRole(RoleDto roleDto);

        public partial ClaimDto ClaimToClaimDto(Claim claim);

        [MapperIgnore(nameof(Claim.Issuer))]
        public partial Claim ClaimDtoToClaim(ClaimDto claimDto);

        public partial SystemSettingValue SystemSettingValueDtoToSystemSettingValue(SystemSettingValueDto organizationData);

        [MapProperty(nameof(SystemSettingValue.SerializedValue), nameof(SystemSettingValueDto.Value))]
        public partial SystemSettingValueDto SystemSettingValueToSystemSettingValueDto(SystemSettingValue organizationData);
    }
}
