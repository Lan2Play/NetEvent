using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetEvent.Server.Models;

public class TicketPurchase
{
    [Key]
    public long? Id { get; set; }

    public long? TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public EventTicketType? Ticket { get; set; }

    public string? PurchaseId { get; set; }

    [ForeignKey(nameof(PurchaseId))]
    public Purchase? Purchase { get; set; }

    public int Price { get; set; }

    public Currency Currency { get; set; }
}
