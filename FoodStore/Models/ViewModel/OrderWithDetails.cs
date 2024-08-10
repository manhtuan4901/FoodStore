namespace FoodStore.Models.ViewModel
{
    public class OrderWithDetails
    {
        public Order Order { get; set; }
        public User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }


}
