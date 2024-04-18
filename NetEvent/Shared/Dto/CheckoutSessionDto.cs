using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class CheckoutSessionDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("sessionData")]
    public string? SessionData { get; set; }
}
