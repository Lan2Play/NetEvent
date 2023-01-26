using System.Security.Claims;
using System.Threading.Tasks;
using Adyen.Model.Checkout;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Data
{
    public interface IPaymentManager
    {
        Task<CreateCheckoutSessionResponse> PayAsync(CartDto cart, ClaimsPrincipal claimsPrincipal);
    }
}
