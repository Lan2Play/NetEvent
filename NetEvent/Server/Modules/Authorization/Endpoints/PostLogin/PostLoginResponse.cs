namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser
{
    public class PostLoginResponse : ResponseBase
    {
        public PostLoginResponse()
        {
        }

        public PostLoginResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
