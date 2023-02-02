using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class CartEntryDto
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("ticketId")]
    public long? TicketId { get; set; }
}
