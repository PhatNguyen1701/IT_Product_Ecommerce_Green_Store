using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public int ProductCount { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<int> Pages { get; set; }
    }
}
