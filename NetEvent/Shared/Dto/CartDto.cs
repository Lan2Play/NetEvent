using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class CartDto
{
    [JsonPropertyName("cartentries")]
    public IReadOnlyList<CartEntryDto> CartEntries { get; set; } = Array.Empty<CartEntryDto>();
}
