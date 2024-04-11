using Newtonsoft.Json;
using Mango.Services.Entities.Dtos;
using Mango.Services.CouponAPI.Interfaces;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");

            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var coupon = JsonConvert.DeserializeObject<CouponDto>(apiContent);
                return coupon;
            }
            return new CouponDto();
        }
    }
}
