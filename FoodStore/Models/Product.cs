using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodStore.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty; // Ensure non-null
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty; // Ensure non-null
        [BsonElement("price")]
        public decimal Price { get; set; } // Ensure non-null
        [BsonElement("quantity")]
        public int Quantity { get; set; } // Ensure non-null
        [BsonElement("images")]
        public string Images { get; set; } = string.Empty; // Ensure non-null
        [BsonElement("categoryId")]
        public string CategoryId { get; set; } = string.Empty; // Ensure non-null

        public OrderDetail? Detail { get; set; }
    }
}
