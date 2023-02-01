using System.Threading.Tasks;
using Blazored.LocalStorage;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public class CartService : ICartService
    {
        private const string _CartKey = "cart";

        private readonly ILocalStorageService _LocalStorage;

        public CartService(ILocalStorageService localStorage)
        {
            _LocalStorage = localStorage;
        }

        public async Task<CartDto> LoadCart(string cartId)
        {
            if (await _LocalStorage.ContainKeyAsync(cartId).ConfigureAwait(false))
            {
                return await _LocalStorage.GetItemAsync<CartDto>(_CartKey);
            }

            var cart = new CartDto();
            await _LocalStorage.SetItemAsync(_CartKey, cart);
            return cart;
        }

        //public async Task<CartDto> AddToCart()
    }
}
