namespace NetEvent.Server.Modules.Roles.Endpoints.PostRole
{
    public class PostRoleResponse : ResponseBase<string>
    {
        public PostRoleResponse(string id) : base(id)
        {
        }

        public PostRoleResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
