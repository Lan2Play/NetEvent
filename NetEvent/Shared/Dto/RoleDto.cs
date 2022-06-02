using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class RoleDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;//"not set";

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("claims")]
    public IEnumerable<string>? Claims { get; set; } = new HashSet<string>();
}
