namespace ITProductECommerce.ViewModels
{
    public class CheckoutVM
    {
        public bool IsCustomer { get; set; }
        public string? ReceiverName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
    }
}
