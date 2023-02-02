﻿using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Adyen.Model.Checkout;
using MediatR;
using NetEvent.Server.Data;
using NetEvent.Server.Data.Events;
using NetEvent.Shared;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Payment.Endpoints
{
    public static class PostCart
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
                var sessionResponse = await _PaymentManager.PayAsync(request.CartDto, request.User);
                var result = sessionResponse.ToCheckoutSessionDto();

                return new Response(result);
            }
        }

        public sealed class Request : IRequest<Response>
        {
            public Request(CartDto cartDto, ClaimsPrincipal user)
            {
                CartDto = cartDto;
                User = user;
            }

            public CartDto CartDto { get; }

            public ClaimsPrincipal User { get; }
        }

        public sealed class Response : ResponseBase<CheckoutSessionDto>
        {
            public Response(CheckoutSessionDto response) : base(response)
            {
            }

            public Response(ReturnType returnType, string error) : base(returnType, error)
            {
            }
        }
    }
}
