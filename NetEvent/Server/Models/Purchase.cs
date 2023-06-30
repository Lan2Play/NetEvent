using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetEvent.Server.Models;

public class Purchase
{
    [Key]
    public string? Id { get; set; }

    public DateTime? PurchaseTime { get; set; }

    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [InverseProperty(nameof(TicketPurchase.Purchase))]
    public IList<TicketPurchase>? TicketPurchases { get; set; }

    public int Price => TicketPurchases?.Sum(x => x.Price) ?? 0;
}
