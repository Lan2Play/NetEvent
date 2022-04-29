namespace NetEvent.Server.Modules
{
    public class ResponseBase : ResponseBase<object>
    {
        public ResponseBase() : base(null)
        {
        }

        public ResponseBase(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
