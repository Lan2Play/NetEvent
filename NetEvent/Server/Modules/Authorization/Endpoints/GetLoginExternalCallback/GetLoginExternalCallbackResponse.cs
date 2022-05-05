using Microsoft.AspNetCore.Http;

namespace NetEvent.Server.Modules.Authorization.Endpoints.GetLoginExternalCallback
{
    public class GetLoginExternalCallbackResponse : ResponseBase<IResult>
    {
        public GetLoginExternalCallbackResponse(IResult value) : base(value)
        {
        }

        public GetLoginExternalCallbackResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
