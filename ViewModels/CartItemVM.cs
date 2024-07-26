namespace ITProductECommerce.ViewModels
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total => Quantity * Price;
        public double TotalPanel { get; set; }
    }
}
