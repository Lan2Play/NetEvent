using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEvent.Server.Models;

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
