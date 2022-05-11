
namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterResponse : ResponseBase
    {
        public PostRegisterResponse()
        {
        }

        public PostRegisterResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
