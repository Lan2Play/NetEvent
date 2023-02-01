using System.Threading.Tasks;
using NetEvent.Shared.Dto;

namespace NetEvent.Client.Services
{
    public interface ICartService
    {
        Task<CartDto> LoadCart(string cartId);
    }
}
