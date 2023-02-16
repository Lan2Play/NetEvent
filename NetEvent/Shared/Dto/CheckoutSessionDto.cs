using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace NetEvent.Shared.Dto;

public class CheckoutSessionDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("sessionData")]
    public string? SessionData { get; set; }
}
