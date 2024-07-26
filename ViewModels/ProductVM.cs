using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Alias { get; set; }
        public string? ImageURL { get; set; }
        public string CurrentImage { get; set; } = "";
        public double UnitPrice { get; set; }
        public string UnitDescription { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public double Discount { get; set; }
        public int Viewed { get; set; }
        public string? ProviderId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Search { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public string? SortBy { get; set; }
        public int RangeInput { get; set; }
        public int UnitInStock { get; set; }
    }
}
