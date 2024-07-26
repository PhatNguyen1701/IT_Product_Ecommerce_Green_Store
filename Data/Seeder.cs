using ITProductECommerce.Helpers;
using System.Xml.Serialization;

namespace ITProductECommerce.Data
{
    public class Seeder
    {
        public static void Initialize(ITProductCommerceContext context)
        {
            context.Database.EnsureCreated();

            if(!context.Webs.Any())
            {
                var web = new Web
                {
                    WebName = "Home Page",
                    URL = "N/A",
                };
                context.Webs.Add(web);
            }

            if(!context.Departments.Any())
            {
                var departments = new Department
                {
                    DepartmentId = "HOD",
                    DepartmentName = "Head of department",
                    Description = "Has ultimate authority and manages all other departments and employees"
                };
                context.Departments.Add(departments);
            }

            if(!context.Roles.Any())
            {
                var role = new Role
                {
                    DepartmentId = "HOD",
                    RoleName = "Admin",
                    WebId = 1,
                    IsAdd = false,
                    IsEdit = false,
                    IsDelete = false,
                    IsViewed = false
                };
                context.Roles.Add(role);
            }

            if(!context.Customers.Any(c => c.CustomerId == "sysadmin"))
            {
                string adminId = "sysadmin";
                string adminPW = "password";
                string randomKey = Util.GenerateRandomKey();

                var admin = new Customer
                {
                    CustomerId = adminId,
                    RandomKey = randomKey,
                    Password = adminPW.ToMd5Hash(randomKey),
                    IsActive = true,
                    Role = 1,
                    CustomerName = "Admin",
                    Gender = true,
                    DoB = DateTime.Now,
                    Address = "N/A",
                    PhoneNumber = "1234567890",
                    Email = "sysadmin@test.com",
                    AvatarURL = "admin-avatar-img",
                };
                context.Customers.Add(admin);
            }

            context.SaveChanges();
        }
    }
}
