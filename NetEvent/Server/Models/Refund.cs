using System;
using System.ComponentModel.DataAnnotations;

namespace NetEvent.Server.Models;

public class Refund
{
    [Key]
    public long? Id { get; set; }

    public DateTime RefundDate { get; set; }
}
