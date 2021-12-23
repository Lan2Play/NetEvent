namespace NetEvent.Client.Models
{
    public class User
    {
        public User(string id, string userName, string firstName, string lastName, string email, IReadOnlyList<byte>? profileImage)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ProfileImage = profileImage;
        }

        public string Id { get; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IReadOnlyList<byte>? ProfileImage { get; set; }
    }
}
