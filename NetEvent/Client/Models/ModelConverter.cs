namespace NetEvent.Client.Models
{
    public static class ModelConverter
    {
        #region User

        public static User ToUser(this IUserAdded_UserAdded userAdded)
        {
            return new User(userAdded.Id ?? throw new ArgumentOutOfRangeException(nameof(userAdded)),
                            userAdded.UserName ?? throw new ArgumentOutOfRangeException(nameof(userAdded)));
        }

        public static User ToUser(this IGetUsers_Users userAdded)
        {
            return new User(userAdded.Id ?? throw new ArgumentOutOfRangeException(nameof(userAdded)),
                            userAdded.UserName ?? throw new ArgumentOutOfRangeException(nameof(userAdded)));
        }

        #endregion
    }
}
