using System.Collections.Generic;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemImage
{
    public class PostSystemImageResponse : ResponseBase<IReadOnlyList<string>>
    {
        public PostSystemImageResponse(IReadOnlyList<string> imageIds) : base(imageIds)
        {
        }

        public PostSystemImageResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
