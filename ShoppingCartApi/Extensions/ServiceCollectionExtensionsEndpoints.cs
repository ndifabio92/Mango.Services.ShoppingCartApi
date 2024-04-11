using ShoppingCartApi.Endpoints.Base;

namespace ShoppingCartApi.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            var endpointType = typeof(IEndpoint);

            var endpoints = endpointType.Assembly
                                        .GetTypes()
                                        .Where(type => !type.IsInterface && type.GetInterfaces().Contains(endpointType));

            foreach (var endpoint in endpoints)
            {
                services.AddTransient(typeof(IEndpoint), endpoint);
            }

            return services;
        }
    }
}
