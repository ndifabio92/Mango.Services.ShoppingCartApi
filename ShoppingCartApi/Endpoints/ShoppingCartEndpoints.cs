using Mango.Services.Business.Interfaces;
using Mango.Services.Entities.Dtos;
using ShoppingCartApi.Endpoints.Base;

namespace ShoppingCartApi.Endpoints
{
    public class ShoppingCartEndpoints: IEndpoint
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("api/ShoppingCart/").WithTags("Shopping Cart");

            group.MapGet("GetCart/{userId}", async (IShoppingCartBusiness shoppingCartBusiness, string userId) =>
            {
                return await shoppingCartBusiness.GetCart(userId);
            })
            .Produces<CartDto>()
            .WithName("GetCartByUser");

            group.MapPost("CartUpsert", async (IShoppingCartBusiness shoppingCartBusiness, CartDto cart) =>
            {
                return await shoppingCartBusiness.CartUpsert(cart);
            })
            .Produces<CartDto>()
            .WithName("CartUpsert");

            group.MapPost("ApplyCoupon", async (IShoppingCartBusiness shoppingCartBusiness, CartDto cart) =>
            {
                return await shoppingCartBusiness.ApplyCoupon(cart);
            })
            .Produces<CartDto>()
            .WithName("ApplyCoupon");

            group.MapPost("RemoveCoupon", async (IShoppingCartBusiness shoppingCartBusiness, CartDto cart) =>
            {
                return await shoppingCartBusiness.RemoveCoupon(cart);
            })
            .Produces<bool>()
            .WithName("RemoveCoupon");

            group.MapDelete("RemoveCart/{id:int}", async (IShoppingCartBusiness shoppingCartBusiness, int id) =>
            {
                return await shoppingCartBusiness.RemoveCart(id);
            })
            .Produces<bool>()
            .WithName("RemoveCart");
        }
    }
}
