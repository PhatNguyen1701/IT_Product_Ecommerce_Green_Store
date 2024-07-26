namespace ITProductECommerce.Data
{
    public class Department
    {
        public string DepartmentId { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string? Description { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
