namespace NetEvent.Shared.Dto.Event
{
    public class EventDto
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public LocationDto? Location { get; set; }

        public string? Description { get; set; }
    }
}
