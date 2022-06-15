namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterUserResponse : ResponseBase
    {
        public PostRegisterUserResponse()
        {
        }

        public PostRegisterUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
