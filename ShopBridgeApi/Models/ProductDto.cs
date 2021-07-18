using System.ComponentModel.DataAnnotations;

namespace ShopBridgeApi.Models
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
