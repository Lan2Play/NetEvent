using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEvent.Server.Models;

public class Payment
{
    [Key]
    public long? Id { get; set; }

    public DateTime PaymentDate { get; set; }

    public long? PurchaseId { get; set; }

    [ForeignKey(nameof(PurchaseId))]
    public Purchase? Purchase { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Payed,
}

public class Refund
{
    [Key]
    public long? Id { get; set; }

    public DateTime RefundDate { get; set; }
}

public enum RefundStatus
{
    Pending,
    Refunded,
}

public class TicketPurchaseRefund
{
    [Key]
    public long? Id { get; set; }

    public long? RefundId { get; set; }

    [ForeignKey(nameof(RefundId))]
    public Refund? Refund { get; set; }

    public long? TicketPurchaseId { get; set; }

    [ForeignKey(nameof(TicketPurchaseId))]
    public TicketPurchase? TicketPurchase { get; set; }
}

public class Purchase
{
    [Key]
    public long? Id { get; set; }

    public DateTime? PurchaseTime { get; set; }

    public long? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }
}

public class TicketPurchase
{
    [Key]
    public long? Id { get; set; }

    public long? TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public EventTicketType? Ticket { get; set; }

    public long? PurchaseId { get; set; }

    [ForeignKey(nameof(PurchaseId))]
    public Purchase? Purchase { get; set; }

    public int Price { get; set; }

    public Currency Currency { get; set; }
}

public class EventTicketType
{
    [Key]
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public int Price { get; set; }

    public Currency Currency { get; set; }

    public long AvailableTickets { get; set; }

    public DateTime SellStartDate { get; set; }

    public DateTime SellEndDate { get; set; }

    public bool IsGiftable { get; set; }

    public long? EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public Event? Event { get; set; }
}

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

public class EventParticipant
{
    [Key]
    public long? Id { get; set; }

    public DateTime Created { get; set; }

    public long? TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public EventTicket? Ticket { get; set; }
}

public enum Currency
{
    Euro,
}
