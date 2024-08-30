using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class DiscountProgramVM
    {
        public int DiscountId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CouponCode { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public string BannerImg { get; set; } = "";
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public string? Search { get; set; }
        public IEnumerable<DiscountProgram> DiscountPrograms { get; set; }
    }
}
