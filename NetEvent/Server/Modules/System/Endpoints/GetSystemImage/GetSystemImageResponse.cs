using Microsoft.AspNetCore.Http;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImage
{
    public class GetSystemImageResponse : ResponseBase<IResult>
    {
        public GetSystemImageResponse(IResult fileResult) : base(fileResult)
        {
        }

        public GetSystemImageResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
