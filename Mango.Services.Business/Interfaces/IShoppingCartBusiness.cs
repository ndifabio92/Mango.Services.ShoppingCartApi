using Mango.Services.Entities.Dtos;

namespace Mango.Services.Business.Interfaces
{
    public interface IShoppingCartBusiness
    {
        public Task<CartDto> GetCart(string userId);
        public Task<CartDto> CartUpsert(CartDto cart);
        public Task<bool> RemoveCart(int cartDetailsId);
        public Task<bool> ApplyCoupon(CartDto cart);
        public Task<bool> RemoveCoupon(CartDto cart);
    }
}
