using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class PaymentMethodDto
{
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }

    [JsonPropertyName("brands")]
    public List<string> Brands { get; set; }

    [JsonPropertyName("configuration")]
    public Dictionary<string, string> Configuration { get; set; }

    [JsonPropertyName("group")]
    public PaymentMethodGroupDto Group { get; set; }

    [JsonPropertyName("issuers")]
    public List<PaymentMethodIssuerDto> Issuers { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
