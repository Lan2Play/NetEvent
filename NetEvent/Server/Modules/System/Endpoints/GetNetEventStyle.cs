using System;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using NetEvent.Server.Data;
using NetEvent.Server.Models;
using NetEvent.Shared;
using NetEvent.Shared.Config;

namespace NetEvent.Server.Modules.System.Endpoints
{

    public static class GetNetEventStyle
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ApplicationDbContext _ApplicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _ApplicationDbContext = applicationDbContext;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var requestedSettings = new SystemSettings.StyleData().Settings.Select(x => x.Key).ToList();
                var requestedStyles = _ApplicationDbContext.Set<SystemSettingValue>().Where(x => x.Key != null && requestedSettings.Contains(x.Key)).ToList();
                var styleBuilder = new StringBuilder();
                styleBuilder.Append(":root {");
                foreach (var requestedStyle in requestedStyles)
                {
                    if (!string.IsNullOrEmpty(requestedStyle.SerializedValue))
                    {
                        AppendVariable(styleBuilder, requestedStyle.Key, requestedStyle.SerializedValue);
                    }
                }

                styleBuilder.Append('}');

                return new Response(Results.File(Encoding.UTF8.GetBytes(styleBuilder.ToString()), "text/css", lastModified: DateTime.Now));
            }

            private static void AppendVariable(StringBuilder styleBuilder, string? key, string serializedValue)
            {
                if (key == null)
                {
                    return;
                }

                var cssVariable = SystemSettings.StyleData.GetCssVariable(key);
                if (!string.IsNullOrEmpty(cssVariable))
                {
                    styleBuilder.Append(cssVariable).Append(": ").Append(serializedValue).Append("!important; ");
                }
            }
        }

        public class Request : IRequest<Response>
        {
            public Request()
            {
            }
        }

        public class Response : ResponseBase<IResult>
        {
            public Response(IResult fileResult) : base(fileResult)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
