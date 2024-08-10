using FoodStore.Models;
using Newtonsoft.Json;

namespace FoodStore.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public List<CartItem> GetCart()
        {
            var cart = Session.GetString("Cart");
            return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        public void SaveCart(List<CartItem> cart)
        {
            Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        public void AddToCart(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.Find(item => item.Product.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { Product = product, Quantity = quantity });
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(string productId)
        {
            var cart = GetCart();
            cart.RemoveAll(item => item.Product.Id == productId);
            SaveCart(cart);
        }

        public void ClearCart()
        {
            SaveCart(new List<CartItem>());
        }


        public int GetTotalQuantity()
        {
            var cart = GetCart();
            return cart.Sum(item => item.Quantity);
        }
    }

}
