using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetEvent.Server.Models;

public class Refund
{
    [Key]
    public long? Id { get; set; }

    public DateTime RefundDate { get; set; }
}
