using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;
using Json = System.Text.Json;

namespace NetEvent.Server.Modules.Payment.Endpoints
{
    public static class PostDropInData
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IPaymentManager _PaymentManager;

            public Handler(IPaymentManager paymentManager)
            {
                _PaymentManager = paymentManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var paymentResponse = await _PaymentManager.SubmitDropInEventDataAsync(request.User, request.PurchaseId, request.PaymentMethodData);

                var paymentResponseJson = Json.JsonSerializer.Serialize(paymentResponse);
                var result = new PaymentResponseDto { PaymentResponseJson = paymentResponseJson };
                return new Response(result);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(ClaimsPrincipal user, string purchaseId, string paymentMethodData)
            {
                User = user;
                PurchaseId = purchaseId;
                PaymentMethodData = paymentMethodData;
            }

            public ClaimsPrincipal User { get; }

            public string PaymentMethodData { get; internal set; }

            public string PurchaseId { get; internal set; }
        }

        public sealed class Response : ResponseBase<PaymentResponseDto>
        {
            public Response(PaymentResponseDto response) : base(response)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
