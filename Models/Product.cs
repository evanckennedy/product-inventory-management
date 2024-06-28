using System.ComponentModel.DataAnnotations;

namespace product_inventory_management.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }
    }
}
