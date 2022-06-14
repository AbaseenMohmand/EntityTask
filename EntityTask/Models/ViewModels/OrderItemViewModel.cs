namespace EntityTask.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public Order Order { get; set; }
        public IList<OrderItem> ItemsWithinOrder { get; set; }
        public IList<Item> AllItems { get; set; }
    }
}
