using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class PaymentMethodDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
