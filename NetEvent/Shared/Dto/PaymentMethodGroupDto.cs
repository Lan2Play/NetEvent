using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class PaymentMethodGroupDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("paymentMethodData")]
    public string PaymentMethodData { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
