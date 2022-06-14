using System.ComponentModel.DataAnnotations;

namespace EntityTask.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public String OrderName { get; set; }
        public virtual List<OrderItem> OrderItem { get; set; }
    }
}
