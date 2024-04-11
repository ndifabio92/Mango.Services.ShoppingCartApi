using System.ComponentModel.DataAnnotations.Schema;
using Mango.Services.Entities.Base;
using Mango.Services.Entities.Dtos;

namespace Mango.Services.Entities
{
    public class CartDetails: EntityBase
    {
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
