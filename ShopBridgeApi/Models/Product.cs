using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Models
{
    [Table("Product")]
    public class Product
    {
        [Required]
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Product)obj;
            return Id == other.Id
                    && Name == other.Name
                    && Price == other.Price
                    && Description == other.Description;
        }
    }
}
