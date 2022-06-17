namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterExternalCompleteResponse : ResponseBase
    {
        public PostRegisterExternalCompleteResponse()
        {
        }

        public PostRegisterExternalCompleteResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
