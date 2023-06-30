using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetEvent.Server.Models;

public class EventTicket
{
    [Key]
    public long? Id { get; set; }

    public long? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    public long? TicketPurchaseId { get; set; }

    [ForeignKey(nameof(TicketPurchaseId))]
    public TicketPurchase? TicketPurchase { get; set; }

    public long? GiftedTicketId { get; set; }

    [ForeignKey(nameof(GiftedTicketId))]
    public EventTicket? GiftedTicket { get; set; }
}
