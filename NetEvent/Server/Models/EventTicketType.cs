using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetEvent.Server.Models;

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
