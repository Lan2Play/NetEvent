using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto.Administration;

public class AdminUserDto : UserDto
{
    [JsonPropertyName("role")]
    public RoleDto Role { get; set; } = default!;
}
