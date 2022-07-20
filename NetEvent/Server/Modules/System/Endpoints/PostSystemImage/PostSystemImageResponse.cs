using System.Collections.Generic;

namespace NetEvent.Server.Modules.System.Endpoints.PostSystemImage
{
    public class PostSystemImageResponse : ResponseBase<string>
    {
        public PostSystemImageResponse(string imageId) : base(imageId)
        {
        }

        public PostSystemImageResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
