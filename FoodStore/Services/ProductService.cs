using FoodStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace FoodStore.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>("Products");
        }

        public void AddProduct(Product product)
        {
            _products.InsertOne(product);
        }

        public List<Product> GetProducts(int limit = 8)
        {
            return _products.Find(product => true).Limit(limit).ToList();
        }


        public Product GetProductById(string id)
        {
            return _products.Find(product => product.Id == id).FirstOrDefault();
        }

        public void UpdateProduct(string id, Product product)
        {
            _products.ReplaceOne(p => p.Id == id, product);
        }

        public void DeleteProduct(string id)
        {
            _products.DeleteOne(product => product.Id == id);
        }

        public List<Product> GetProductsByCategory(string categoryId)
        {
            return _products.Find(product => product.CategoryId == categoryId).ToList();
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var filter = Builders<Product>.Filter.Regex("name", new BsonRegularExpression(searchTerm, "i"));
            return _products.Find(filter).ToList();
        }




        public long CountAllProducts()
        {
            return _products.CountDocuments(new BsonDocument());
        }

        public List<Product> GetProductsPaginated(int page, int pageSize)
		{
			return _products.Find(product => true)
							 .Skip((page - 1) * pageSize)
							 .Limit(pageSize)
							 .ToList();
		}

	}
}
