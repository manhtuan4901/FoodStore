using System.ComponentModel.DataAnnotations;

namespace FoodStore.Areas.Admin.Models
{
    public class ProductVM
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Images { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
    }
}

