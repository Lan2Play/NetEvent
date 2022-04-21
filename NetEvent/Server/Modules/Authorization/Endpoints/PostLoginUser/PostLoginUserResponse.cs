namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLoginUser
{
    public class PostLoginUserResponse : ResponseBase
    {
        public PostLoginUserResponse()
        {
        }

        public PostLoginUserResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
