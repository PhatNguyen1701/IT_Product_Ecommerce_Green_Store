namespace ITProductECommerce.Data
{
    public class DiscountProgram
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

        public ICollection<Customer> Customers { get; set; }
    }
}
