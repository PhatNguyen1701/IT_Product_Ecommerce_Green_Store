namespace ITProductECommerce.Data
{
    public class Web
    {
        public int WebId { get; set; }
        public string WebName { get; set; } = "";
        public string URL { get; set; } = "";

        public ICollection<Role> Roles { get; set; }
    }
}
