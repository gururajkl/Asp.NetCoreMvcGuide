namespace Bulky.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public IEnumerable<OrderDetail>? OrderDetail { get; set; }
    }
}
