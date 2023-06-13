﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Services
{
    public interface IPaymentService
    {
        Task<ServiceResult<PurchaseDto?>> BuyTicketAsync(long id, int amount, CancellationToken cancellationToken);

        Task<CartDto> LoadCart(string cartId);

        Task<ServiceResult<IReadOnlyCollection<PaymentMethodDto>?>> LoadPaymentMethodsAsync(long amount, CurrencyDto currency, CancellationToken cancellationToken);

        Task<ServiceResult<PaymentResponseDto>> MakePaymentAsync(string purchaseId, string paymentDataJson, CancellationToken cancellationToken);
    }
}