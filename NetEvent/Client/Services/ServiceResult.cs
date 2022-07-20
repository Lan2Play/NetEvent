using System.Diagnostics.CodeAnalysis;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class ServiceResult
    {
        protected ServiceResult(bool successful, string? messageKey)
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
