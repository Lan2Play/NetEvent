namespace NetEvent.Client.Services
{
    public class ServiceResult<T> : ServiceResult
    {
        private ServiceResult(T? data, bool successful, string? messageKey) : base(successful, messageKey)
        {
            ResultData = data;
        }

        public static ServiceResult<T> Success(T data, string? messageKey = null) => new ServiceResult<T>(data, true, messageKey);

        public static new ServiceResult<T> Error(string messageKey) => new ServiceResult<T>(default, false, messageKey);

        public T? ResultData { get; }
    }
}
