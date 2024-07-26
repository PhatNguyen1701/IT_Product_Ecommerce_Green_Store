namespace ITProductECommerce.Data
{
    public class Provider
    {
        public string ProviderId { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string LogoURL { get; set; } = "";
        public string? Representative { get; set; }
        public string Email { get; set; } = "";
        public string Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
