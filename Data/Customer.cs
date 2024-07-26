namespace ITProductECommerce.Data
{
    public class Customer
    {
        public string CustomerId { get; set; } = "";
        public string? Password { get; set; }
        public string CustomerName { get; set; } = "";
        public bool Gender { get; set; }
        public DateTime? DoB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = "";
        public string? AvatarURL { get; set; }
        public bool IsActive { get; set; }
        public int Role { get; set; }
        public string? RandomKey { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<MainComment> MainComments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
}
