namespace ITProductECommerce.Data
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string StaffId { get; set; } = "";
        public string DepartmentId { get; set; } = "";
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }

        public User User { get; set; }
        public Department Department { get; set; }
    }
}
