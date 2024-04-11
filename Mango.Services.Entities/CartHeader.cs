using System.ComponentModel.DataAnnotations.Schema;
using Mango.Services.Entities.Base;

namespace Mango.Services.Entities
{
    public class CartHeader: EntityBase
    {
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        [NotMapped]
        public double Discount { get; set; }
        [NotMapped]
        public double Total { get; set; }
    }
}