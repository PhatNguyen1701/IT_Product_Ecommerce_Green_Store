namespace ITProductECommerce.Data
{
    public class Role
    {
        public int RoleId { get; set; }
        public string DepartmentId { get; set; } = "";
        public string RoleName { get; set; } = "";
        public int WebId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsViewed { get; set; }

        public Department Department { get; set; }
        public Web Web { get; set; }
        public ICollection<Staff> Staffs { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
