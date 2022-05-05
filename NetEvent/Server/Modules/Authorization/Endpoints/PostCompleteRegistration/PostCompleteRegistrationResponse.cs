
namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostCompleteRegistrationResponse : ResponseBase
    {
        public PostCompleteRegistrationResponse()
        {
        }

        public PostCompleteRegistrationResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
