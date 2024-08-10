using FoodStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodStore.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        public void AddUser(User user)
        {
            _users.InsertOne(user);
        }

        public User GetUserByUsername(string username)
        {
            return _users.Find(user => user.Username == username).FirstOrDefault();
        }

        public User GetUserById(string id)
        {
            return _users.Find(user => user.Id == id).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Email, user.Email)
                .Set(u => u.Fullname, user.Fullname)
                .Set(u => u.Phone, user.Phone)
                .Set(u => u.Address, user.Address)
                .Set(u => u.Username, user.Username)
                .Set(u => u.Password, user.Password);

            _users.UpdateOne(filter, update);
        }

        public void AddRoleToUser(string userId, string role)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.AddToSet(u => u.Roles, role);
            _users.UpdateOne(filter, update);
        }

        public List<string> GetUserRoles(string userId)
        {
            var user = _users.Find(user => user.Id == userId).FirstOrDefault();
            return user?.Roles ?? new List<string>();
        }

        public List<User> GetAllUsers()
        {
            return _users.Find(_ => true).ToList();
        }

        public void DeleteUser(User user)
        {
            _users.DeleteOne(u => u.Id == user.Id);
        }

        public bool UserExistsByUsername(string username)
        {
            return _users.Find(user => user.Username == username).Any();
        }

        public bool UserExistsByEmail(string email)
        {
            return _users.Find(user => user.Email == email).Any();
        }

    }
}

