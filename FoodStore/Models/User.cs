using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FoodStore.Models
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
        [BsonElement("email")]
        [EmailAddress]
        public string? Email { get; set; }
        [BsonElement("fullname")]
        public string? Fullname { get; set; } = string.Empty;
        [BsonElement("phone")]
        public string? Phone { get; set; } = string.Empty;
        [BsonElement("address")]
        public string? Address { get; set; } = string.Empty;
        [BsonElement("username")]
        public string? Username { get; set; }
        [BsonElement("password")]
        public string? Password { get; set; }
        [BsonElement("roles")]
        public List<string>? Roles { get; set; }
    }
}
