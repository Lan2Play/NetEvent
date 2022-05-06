using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class ClaimDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}
