namespace NetEvent.Server.Modules.System.Endpoints.PostSystemSetting
{
    public class PostSystemSettingsResponse : ResponseBase
    {
        public PostSystemSettingsResponse()
        {
        }

        public PostSystemSettingsResponse(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }
}
