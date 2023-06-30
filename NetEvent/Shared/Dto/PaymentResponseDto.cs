using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class PaymentResponseDto
{
    [JsonPropertyName("paymentResponseJson")]
    public string PaymentResponseJson { get; set; }
}
