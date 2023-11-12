namespace Bulky.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }
        public double OrderTotal { get; set; }
    }
}
