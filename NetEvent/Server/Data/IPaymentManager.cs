using System.Security.Claims;
using System.Threading.Tasks;
using Adyen.Model.Checkout;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Data
{
    public interface IPaymentManager
    {
        Task<PaymentMethodsResponse> GetPaymentMethodsAsync(long amount, CurrencyDto currency);

        Task<CreateCheckoutSessionResponse> PayAsync(CartDto cart, ClaimsPrincipal claimsPrincipal);
    }
}
