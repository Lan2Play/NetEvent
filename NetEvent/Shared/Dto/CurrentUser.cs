using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class CurrentUserDto : UserDto
{
    [JsonPropertyName("isauthenticated")]
    public bool IsAuthenticated { get; set; }

    [JsonPropertyName("claims")]
    public Dictionary<string, string> Claims { get; set; }

}
