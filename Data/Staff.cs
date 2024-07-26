namespace ITProductECommerce.Data
{
    public class Staff
    {
        public string StaffId { get; set; } = "";
        public string StaffName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
