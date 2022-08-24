namespace NetEvent.Client.Extensions
{
    public static class UrlHelper
    {
        public static string GetEventLink(object id, bool edit = true)
        {
            if (edit)
            {
                return $"/administration/event/{id}";
            }

            return $"/event/{id}";
        }
    }
}
