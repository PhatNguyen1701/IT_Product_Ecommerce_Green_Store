namespace ITProductECommerce.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
