namespace ITProductECommerce.Data
{
    public class Staff
    {
        public string StaffId { get; set; } = "";
        public string StaffName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }
        public string? RandomKey { get; set; }
        public bool Gender { get; set; }
        public DateTime? DoB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string AvatarURL { get; set; } = "";
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public Role Role { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
