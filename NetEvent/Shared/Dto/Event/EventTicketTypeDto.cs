using System;

namespace NetEvent.Shared.Dto.Event
{

    public class EventTicketTypeDto
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? Slug { get; set; }

        public int Price { get; set; }

        public CurrencyDto Currency { get; set; }

        public long AvailableTickets { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime SellEndDate { get; set; }

        public bool IsGiftable { get; set; }
    }
}
