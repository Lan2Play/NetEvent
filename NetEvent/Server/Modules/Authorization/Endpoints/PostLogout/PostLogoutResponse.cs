namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser;

public class PostLogoutResponse : ResponseBase
{
    public PostLogoutResponse()
    {
    }

    public PostLogoutResponse(ReturnType returnType, string error) : base(returnType, error)
    {
    }
}
