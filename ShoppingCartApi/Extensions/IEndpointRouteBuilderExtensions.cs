using ShoppingCartApi.Endpoints.Base;

namespace ShoppingCartApi.Extensions
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static void UseApplicationEndpoints(this WebApplication builder)
        {
            var scope = builder.Services.CreateScope();

            var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                endpoint.AddRoutes(builder);
            }
        }
    }
}