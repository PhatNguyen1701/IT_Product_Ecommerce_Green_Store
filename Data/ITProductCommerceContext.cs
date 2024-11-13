using ITProductECommerce.Configurations;
using ITProductECommerce.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Data
{
    public class ITProductCommerceContext : IdentityDbContext<User, Role, string>
    {
        public ITProductCommerceContext(DbContextOptions<ITProductCommerceContext> options)
            : base(options)
        {

        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Web> Webs { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<DiscountProgram> DiscountPrograms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new WebConfiguration());
            modelBuilder.ApplyConfiguration(new MainCommentConfiguration());
            modelBuilder.ApplyConfiguration(new SubCommentConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountProgramConfiguration());

            #region Seeding using Migration
            //modelBuilder.Entity<Web>().HasData(new Web
            //{
            //    WebId = 1,
            //    WebName = "Home Page",
            //    URL = "N/A",
            //});

            //modelBuilder.Entity<Department>().HasData(new Department
            //{
            //    DepartmentId = "HOD",
            //    DepartmentName = "Head of department",
            //    Description = "Has ultimate authority and manages all other departments and employees"
            //});

            //modelBuilder.Entity<Role>().HasData(new Role
            //{
            //    RoleId = 1,
            //    DepartmentId = "HOD",
            //    RoleName = "Admin",
            //    WebId = 1,
            //    IsAdd = false,
            //    IsEdit = false,
            //    IsDelete = false,
            //    IsViewed = false
            //});

            //string adminId = "sysadmin";
            //string adminPW = "password";
            //string randomKey = Util.GenerateRandomKey();

            //var customer = new Customer
            //{
            //    CustomerId = adminId,
            //    RandomKey = randomKey,
            //    Password = adminPW.ToMd5Hash(randomKey),
            //    IsActive = true,
            //    Role = 1,
            //    CustomerName = "Admin",
            //    Gender = true,
            //    DoB = DateTime.Now,
            //    Address = "N/A",
            //    PhoneNumber = "1234567890",
            //    Email = "sysadmin@test.com",
            //    AvatarURL = "",
            //};

            //modelBuilder.Entity<Customer>().HasData(customer); 
            #endregion
        }
    }
}
