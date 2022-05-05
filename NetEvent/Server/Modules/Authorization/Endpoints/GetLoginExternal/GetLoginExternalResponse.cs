using Microsoft.AspNetCore.Http;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternal
{
    public class GetLoginExternalResponse : ResponseBase<IResult>
    {
        public GetLoginExternalResponse(IResult value) : base(value)
        {
        }

        public GetLoginExternalResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
