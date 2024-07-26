namespace ITProductECommerce.Data
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = "";
        public string? Description { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
