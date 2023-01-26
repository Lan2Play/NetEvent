using System.Security.Claims;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;
using NetEvent.Shared.Dto.Event;
using Riok.Mapperly.Abstractions;

namespace NetEvent.Shared
{
    [Mapper]
    public static partial class DtoMapper
    {
        public static partial UserDto ToUserDto(this ApplicationUser applicationUser);

        [MapperIgnoreTarget(nameof(CurrentUserDto.ProfileImage))]
        [MapperIgnoreTarget(nameof(AdminUserDto.Role))]
        public static partial AdminUserDto ToAdminUserDto(this ApplicationUser applicationUser);

        public static partial CurrentUserDto ToCurrentUserDto(this ApplicationUser applicationUser);

        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.IsAuthenticated)}", nameof(CurrentUserDto.IsAuthenticated))]
        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.Name)}", nameof(CurrentUserDto.UserName))]
        [MapperIgnoreTarget(nameof(CurrentUserDto.Claims))]
        public static partial CurrentUserDto ToCurrentUserDto(this ClaimsPrincipal claimsPrincipal);

        [MapperIgnoreTarget(nameof(RoleDto.Claims))]
        public static partial RoleDto ToRoleDto(this ApplicationRole applicationRole);

        [MapperIgnoreTarget(nameof(ApplicationRole.NormalizedName))]
        [MapperIgnoreTarget(nameof(ApplicationRole.ConcurrencyStamp))]
        public static partial ApplicationRole ToApplicationRole(this RoleDto roleDto);

        public static partial ClaimDto ToClaimDto(this Claim claim);

        [MapperIgnoreTarget(nameof(Claim.Issuer))]
        public static partial Claim ToClaim(this ClaimDto claimDto);

        [MapProperty(nameof(SystemSettingValueDto.Value), nameof(SystemSettingValue.SerializedValue))]
        public static partial SystemSettingValue ToSystemSettingValue(this SystemSettingValueDto organizationData);

        [MapProperty(nameof(SystemSettingValue.SerializedValue), nameof(SystemSettingValueDto.Value))]
        public static partial SystemSettingValueDto ToSystemSettingValueDto(this SystemSettingValue organizationData);

        public static partial SystemInfo ToSystemInfo(this SystemInfoDto systemInfo);

        public static partial SystemInfoDto ToSystemInfoDto(this SystemInfo systemInfo);

        public static partial SystemImageDto ToSystemImageDto(this SystemImage systemImage);

        public static partial SystemImageDto SystemImageToSystemImageDto(this SystemImage systemImage);

        public static partial EventDto ToEventDto(this Event eventToConvert);

        public static partial Event ToEvent(this EventDto eventToConvert);

        public static partial EventTicketTypeDto ToEventTicketTypeDto(this EventTicketType eventTicketTypeToConvert);

        public static partial EventTicketType ToEventTicketType(this EventTicketTypeDto eventTicketTypeToConvert);

        public static partial CurrencyDto ToCurrencyDto(this Currency currency);

        public static partial VenueDto ToVenueDto(this Venue venue);

        public static partial Venue ToVenue(this VenueDto venue);
    }
}
