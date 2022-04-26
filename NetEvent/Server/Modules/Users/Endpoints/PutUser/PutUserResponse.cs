namespace NetEvent.Server.Modules.Users.Endpoints.PutUser
{
    public class PutUserResponse : ResponseBase
    {
        public PutUserResponse()
        {
        }

        public PutUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
