using System;

using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class PurchaseDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("purchaseTime")]
    public DateTime? PurchaseTime { get; set; }

    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    //[InverseProperty(nameof(TicketPurchase.Purchase))]
    //public List<TicketPurchase>? TicketPurchases { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }
}
