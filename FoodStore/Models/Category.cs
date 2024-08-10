using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FoodStore.Models
{
	public class Category
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		[BsonElement("name")]
		public string? Name { get; set; }
	}
}
