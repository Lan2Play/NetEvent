using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Shared;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Modules.Payment.Endpoints
{
    public static class GetPaymentMethods
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
                var paymentMethodResponse = await _PaymentManager.GetPaymentMethodsAsync(request.Amount, request.Currency);
                var result = paymentMethodResponse.PaymentMethods.Select(x => x.ToPaymentMethodDto()).ToList();

                return new Response(result);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(long amount, CurrencyDto currency)
            {
                Amount = amount;
                Currency = currency;
            }

            public CurrencyDto Currency { get; }

            public long Amount { get; }
        }

        public sealed class Response : ResponseBase<IReadOnlyCollection<PaymentMethodDto>>
        {
            public Response(IReadOnlyCollection<PaymentMethodDto> response) : base(response)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
