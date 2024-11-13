using Microsoft.AspNetCore.Identity;

namespace ITProductECommerce.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool Gender { get; set; }
        public DateTime? DoB { get; set; }
        public string? Address { get; set; }
        public string? AvatarURL { get; set; }
        public int? DiscountId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStaff { get; set; }
        public bool RememberMe { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<MainComment> MainComments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
    }
}
