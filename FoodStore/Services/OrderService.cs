using FoodStore.Models;
using FoodStore.Models.ViewModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodStore.Services
{
    public class OrderService
	{
		private readonly IMongoCollection<Order> _orders;
		private readonly IMongoCollection<OrderDetail> _orderDetails;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Product> _products;

        public OrderService(IOptions<MongoDBSettings> settings)
		{
			var client = new MongoClient(settings.Value.ConnectionString);
			var database = client.GetDatabase(settings.Value.DatabaseName);
			_orders = database.GetCollection<Order>("Orders");
			_orderDetails = database.GetCollection<OrderDetail>("OrderDetails");
            _users = database.GetCollection<User>("Users");
            _products = database.GetCollection<Product>("Products");
        }

        public async Task<List<OrderWithUserDetails>> GetAllOrdersWithUserDetailsAsync()
        {
            var orders = await _orders.Find(_ => true).ToListAsync();
            List<OrderWithUserDetails> ordersWithDetails = new List<OrderWithUserDetails>();

            foreach (var order in orders)
            {
                var user = await _users.Find(u => u.Id == order.UserId).FirstOrDefaultAsync();
                ordersWithDetails.Add(new OrderWithUserDetails
                {
                    Order = order,
                    User = user
                });
            }

            return ordersWithDetails;
        }

        public void CreateOrder(Order order, List<CartItem> cartItems)
		{
			_orders.InsertOne(order);
			var orderDetails = cartItems.Select(item => new OrderDetail
			{
				OrderId = order.Id,
				ProductId = item.Product.Id,
                ProductName = item.Product.Name,
				Quantity = item.Quantity,
				Price = item.Product.Price
			}).ToList();

			_orderDetails.InsertMany(orderDetails);
		}

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find<Order>(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<OrderWithUserDetails>> GetOrdersWithUserDetailsAsync(string userId)
        {
            var orders = await _orders.Find(o => o.UserId == userId).ToListAsync();
            var users = await _users.Find(u => u.Id == userId).ToListAsync();

            var orderWithDetails = orders.Select(order => new OrderWithUserDetails
            {
                Order = order,
                User = users.FirstOrDefault()
            }).ToList();

            return orderWithDetails;
        }


        public async Task<OrderWithDetails> GetOrderDetailsAsync(string orderId)
        {
            var order = await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null) return null;

            var user = await _users.Find(u => u.Id == order.UserId).FirstOrDefaultAsync();
            var orderDetails = await _orderDetails.Find(od => od.OrderId == orderId).ToListAsync();

            return new OrderWithDetails
            {
                Order = order,
                User = user,
                OrderDetails = orderDetails
            };
        }

        public async Task UpdateOrderStatus(string orderId, OrderStatus newStatus)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
            var update = Builders<Order>.Update.Set(o => o.OrderStatus, newStatus);
            await _orders.UpdateOneAsync(filter, update);
        }




        public async Task DeleteOrder(string orderId)
        {
            await _orders.DeleteOneAsync(o => o.Id == orderId);
        }



    }
}
