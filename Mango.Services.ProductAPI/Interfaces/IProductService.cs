
using Mango.Services.Entities.Dtos;

namespace Mango.Services.ProductAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
