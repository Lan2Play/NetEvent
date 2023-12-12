using System.Security.Claims;
using System.Threading.Tasks;
using Adyen.Model.Checkout;
using NetEvent.Server.Models;
using NetEvent.Shared.Dto;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Server.Data
{
    public interface IPaymentManager
    {
        Task<PaymentMethodsResponse> GetPaymentMethodsAsync(long amount, CurrencyDto currency);

        Task<Purchase> PurchaseAsync(CartDto cart, ClaimsPrincipal claimsPrincipal);

        Task<PaymentResponse?> SubmitDropInEventDataAsync(ClaimsPrincipal claimsPrincipal, string purchaseId, string paymentMethodData);
    }
}
