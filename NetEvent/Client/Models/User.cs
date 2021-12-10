namespace NetEvent.Client.Models
{
    public class User
    {
        public User(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public string Id { get; }

        public string UserName { get; }
    }
}
