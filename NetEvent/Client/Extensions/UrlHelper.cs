namespace NetEvent.Client.Extensions
{
    public static class UrlHelper
    {
        public static string GetEventLink(object id, bool edit)
        {
            if (edit)
            {
                return $"/administration/event/{id}";
            }

            return $"/event/{id}";
        }

        public static string GetVenueLink(object id, bool edit)
        {
            if (edit)
            {
                return $"/administration/venue/{id}";
            }

            return $"/venue/{id}";
        }

        public static string GetTicketTypesLink(object eventId, object id)
        {
            return $"/administration/event/{eventId}/tickettype/{id}";
        }
    }
}
