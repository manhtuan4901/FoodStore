using FoodStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodStore.Services
{
	public class CategoryService
	{
		private readonly IMongoCollection<Category> _category;

		public CategoryService(IOptions<MongoDBSettings> settings)
		{
			var client = new MongoClient(settings.Value.ConnectionString);
			var database = client.GetDatabase(settings.Value.DatabaseName);
			_category = database.GetCollection<Category>("Categories");
		}

		public void AddCategory(Category category)
		{
			_category.InsertOne(category);
		}

		public List<Category> GetCategories()
		{
			return _category.Find(category => true).ToList();
		}

		public Category GetCategoryById(string id)
		{
			return _category.Find(category => category.Id == id).FirstOrDefault();
		}

		public void UpdateCategory(string id, Category category)
		{
			_category.ReplaceOne(c => c.Id == id, category);
		}

		public void DeleteCategory(string id)
		{
			_category.DeleteOne(category => category.Id == id);
		}
	}
}
