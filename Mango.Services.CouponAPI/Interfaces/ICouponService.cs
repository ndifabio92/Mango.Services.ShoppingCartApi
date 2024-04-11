using Mango.Services.Entities.Dtos;

namespace Mango.Services.CouponAPI.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
