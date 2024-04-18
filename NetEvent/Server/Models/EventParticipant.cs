using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEvent.Server.Models;

public class EventParticipant
{
    [Key]
    public long? Id { get; set; }

    public DateTime Created { get; set; }

    public long? TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public EventTicket? Ticket { get; set; }
}
