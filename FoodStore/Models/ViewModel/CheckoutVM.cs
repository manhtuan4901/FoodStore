namespace FoodStore.Models.ViewModel
{
    public class CheckoutVM
    {
        public User? UserInfo { get; set; } 
        public List<CartItem>? CartItems { get; set; } 
        public decimal? TotalAmount { get; set; }
    }
}
