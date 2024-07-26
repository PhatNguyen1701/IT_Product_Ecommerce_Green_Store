namespace ITProductECommerce.Data
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string? Alias { get; set; }
        public int CategoryId { get; set; }
        public string? UnitDescription { get; set; }
        public double UnitPrice { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        public double Discount { get; set; }
        public int Viewed { get; set; }
        public string? Description { get; set; }
        public string ProviderId { get; set; } = "";
        public int UnitInStock { get; set; }
        public int AvgRating { get; set; }

        public Category Category { get; set; }
        public Provider Provider { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<MainComment> MainComments { get; set; }

        public Product()
        {
            MainComments = new List<MainComment>();
        }
    }
}
