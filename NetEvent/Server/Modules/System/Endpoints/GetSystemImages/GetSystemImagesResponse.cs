using System.Collections.Generic;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.System.Endpoints.GetSystemImages
{
    public class GetSystemImagesResponse : ResponseBase<IEnumerable<SystemImageDto>>
    {
        public GetSystemImagesResponse(IEnumerable<SystemImageDto> result) : base(result)
        {
        }

        public GetSystemImagesResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
