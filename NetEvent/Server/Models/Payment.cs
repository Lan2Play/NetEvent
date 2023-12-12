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
