using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class ProductDetailVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public double Price { get; set; }
        public string Discription { get; set; }
        public string CategoryName { get; set; }
        public string ProviderName { get; set; }
        public string Detail { get; set; }
        public int AvgRating { get; set; }
        public int UnitInStock { get; set; }
        public IEnumerable<MainComment> MainComments { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
