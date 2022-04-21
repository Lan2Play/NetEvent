namespace NetEvent.Server.Modules.Authorization.Endpoints.PostLogoutUser;

public class PostLogoutUserResponse : ResponseBase
{
    public PostLogoutUserResponse()
    {
    }

    public PostLogoutUserResponse(ReturnType returnType, string error) : base(returnType, error)
    {
    }
}
