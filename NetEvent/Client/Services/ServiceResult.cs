using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Administration;

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
