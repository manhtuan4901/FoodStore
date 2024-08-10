using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FoodStore.Models
{
	public enum OrderStatus
	{
		Pending,  // 0
		Delivered,  // 1
		Cancelled  // 2
	}

	public class Order
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		[BsonElement("userId")]
		public string? UserId { get; set; }
		[BsonElement("orderDate")]
		public DateTime OrderDate { get; set; }
		[BsonElement("totalAmout")]
		public decimal? TotalAmount { get; set; }

		[BsonElement("orderStatus")]
		public OrderStatus OrderStatus { get; set; }  // Add this line

		public List<OrderDetail> Products { get; set; }
        public string GetStatusText()
        {
            return this.OrderStatus switch
            {
                OrderStatus.Pending => "Pending",
                OrderStatus.Delivered => "Delivered",
                OrderStatus.Cancelled => "Cancelled",
                _ => "Unknown",
            };
        }
    }

	public class OrderDetail
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		[BsonElement("orderId")]
		public string? OrderId { get; set; }
		[BsonElement("productId")]
		public string? ProductId { get; set; }
		[BsonElement("productName")]
        public string? ProductName { get; set; }
        [BsonElement("quantity")]
        public int? Quantity { get; set; }
		[BsonElement("price")]
		public decimal? Price { get; set; }
    }
}
