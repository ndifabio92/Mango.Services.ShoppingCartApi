using Microsoft.EntityFrameworkCore;
using Mango.Services.Business.Base;
using Mango.Services.Business.Interfaces;
using Mango.Services.Data;
using Mango.Services.Entities;
using Mango.Services.Entities.Dtos;
using Mango.Services.CouponAPI.Interfaces;
using Mango.Services.ProductAPI.Interfaces;

namespace Mango.Services.Business
{
    public class ShoppingCartBusiness : BusinessBase, IShoppingCartBusiness
    {
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public ShoppingCartBusiness(DataContext dataContext, ICouponService couponService, IProductService productoService) : base(dataContext)
        {
            _productService = productoService;
            _couponService = couponService;
        }

        public async Task<CartDto> GetCart(string userId)
        {
            var cartDb = await _dataContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

            var cart = new CartDto()
            {
                CartHeader = new()
                {
                    Id = cartDb.Id,
                    UserId = cartDb.UserId,
                    CouponCode = cartDb.CouponCode,
                    Discount = cartDb.Discount,
                },
                CartDetails = await _dataContext.CartDetails.Where(x => x.CartHeaderId == cartDb.Id).Select(x => new CartDetailsDto()
                {
                    Id = x.Id,
                    Count = x.Count,
                    Product = x.Product,
                    CartHeaderId = x.CartHeaderId,
                    ProductId = x.ProductId
                }).ToListAsync()
            };

            var productService = await _productService.GetProducts();

            foreach (var item in cart.CartDetails)
            {
                item.Product = productService.FirstOrDefault(x => x.Id == item.ProductId);
                cart.CartHeader.Total += (item.Count * item.Product.Price);
            }

            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                if (coupon != null && cart.CartHeader.Total > coupon.MinAmount)
                {
                    cart.CartHeader.Total -= coupon.DiscountAmount;
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }

            return cart;
        }

        public async Task<CartDto> CartUpsert(CartDto cart)
        {
            var headerDb = await _dataContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);

            if (headerDb == null)
            {
                var cartHeader = new CartHeader()
                {
                    CouponCode = cart.CartHeader.CouponCode,
                    Discount = cart.CartHeader.Discount,
                    UserId = cart.CartHeader.UserId,
                    Total = cart.CartHeader.Total
                };

                _dataContext.CartHeaders.Add(cartHeader);
                await _dataContext.SaveChangesAsync();

                var cartDetail = new CartDetails()
                {
                    CartHeaderId = cartHeader.Id,
                    Count = cart.CartDetails.FirstOrDefault().Count,
                    Product = cart.CartDetails.FirstOrDefault().Product,
                    ProductId = cart.CartDetails.FirstOrDefault().ProductId
                };

                _dataContext.CartDetails.Add(cartDetail);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                var detailDb = await _dataContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    x => x.Id == cart.CartDetails.First().ProductId &&
                    x.CartHeaderId == headerDb.Id);

                if (detailDb == null)
                {
                    var cartDetail = new CartDetails()
                    {
                        CartHeaderId = headerDb.Id,
                        Count = cart.CartDetails.FirstOrDefault().Count,
                        Product = cart.CartDetails.FirstOrDefault().Product,
                    };

                    _dataContext.CartDetails.Add(cartDetail);
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    var cartDetailUpdate = new CartDetails()
                    {
                        CartHeaderId = cart.CartDetails.FirstOrDefault().CartHeaderId += detailDb.CartHeaderId,
                        Count = cart.CartDetails.FirstOrDefault().Count += detailDb.Count,
                        Product = cart.CartDetails.FirstOrDefault().Product,
                    };

                    _dataContext.CartDetails.Update(cartDetailUpdate);
                    await _dataContext.SaveChangesAsync();
                }
            }

            return cart;
        }

        public async Task<bool> RemoveCart(int cartDetailsId)
        {
            var cartDetails = _dataContext.CartDetails
                   .First(u => u.Id == cartDetailsId);

            int totalCountofCartItem = _dataContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
            _dataContext.CartDetails.Remove(cartDetails);
            if (totalCountofCartItem == 1)
            {
                var cartHeaderToRemove = await _dataContext.CartHeaders
                   .FirstOrDefaultAsync(u => u.Id == cartDetails.CartHeaderId);

                _dataContext.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApplyCoupon(CartDto cart)
        {
            var cartFromDb = await _dataContext.CartHeaders.FirstAsync(u => u.UserId == cart.CartHeader.UserId);
            cartFromDb.CouponCode = cart.CartHeader.CouponCode;
            _dataContext.CartHeaders.Update(cartFromDb);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveCoupon(CartDto cart)
        {
            var cartFromDb = await _dataContext.CartHeaders.FirstAsync(u => u.UserId == cart.CartHeader.UserId);
            cartFromDb.CouponCode = "";
            _dataContext.CartHeaders.Update(cartFromDb);
            await _dataContext.SaveChangesAsync();

            return true;
        }
    }
}