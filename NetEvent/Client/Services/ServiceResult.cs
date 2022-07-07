namespace NetEvent.Client.Services
{
    public class ServiceResult
    {
        private ServiceResult(bool successful, string? messageKey)
        {
            Successful = successful;
            MessageKey = messageKey;
        }

        public static ServiceResult Success(string? messageKey = null) => new ServiceResult(true, messageKey);

        public static ServiceResult Error(string messageKey) => new ServiceResult(false, messageKey);

        public bool Successful { get; }

        public string? MessageKey { get; }
    }
}
