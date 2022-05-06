﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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

        [MapperIgnore(nameof(AdminUserDto.Role))]
        public partial AdminUserDto ApplicaitonUserToAdminUserDto(ApplicationUser applicationUser);

        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.IsAuthenticated)}", nameof(CurrentUserDto.IsAuthenticated))]
        [MapProperty($"{nameof(ClaimsPrincipal.Identity)}.{nameof(ClaimsPrincipal.Identity.Name)}", nameof(CurrentUserDto.UserName))]
        [MapperIgnore(nameof(CurrentUserDto.Claims))]
        public partial CurrentUserDto ClaimsPrincipalToCurrentUserDto(ClaimsPrincipal claimsPrincipal);

        [MapperIgnore(nameof(RoleDto.C))]
        public partial RoleDto IdentityRoleToRoleDto(IdentityRole identityRole);

        public partial ClaimDto ClaimToClaimDto(Claim claim);

        public partial OrganizationData OrganizationDataDtoToOrganizationData(OrganizationDataDto organizationData);

        public partial OrganizationDataDto OrganizationDataToOrganizationDataDto(OrganizationData organizationData);
    }
}
