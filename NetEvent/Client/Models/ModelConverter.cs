namespace NetEvent.Client.Models
{
    public static class ModelConverter
    {
        #region User

        public static User ToUser(this IUser user)
        {
            return new User(user.Id ?? throw new ArgumentOutOfRangeException(nameof(user)),
                            user.UserName ?? throw new ArgumentOutOfRangeException(nameof(user)),
                            user.FirstName ?? throw new ArgumentOutOfRangeException(nameof(user)),
                            user.LastName?? throw new ArgumentOutOfRangeException(nameof(user)),
                            user.Email ?? throw new ArgumentOutOfRangeException(nameof(user)),
                            user.ProfilePicture);
        }

        #endregion
    }
}
