using Microsoft.AspNetCore.Identity;

namespace ITProductECommerce.Data
{
    public class Role : IdentityRole
    {
        public string DepartmentId { get; set; } = "";
        public int WebId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsViewed { get; set; }

        public Department Department { get; set; }
        public Web Web { get; set; }
    }
}
