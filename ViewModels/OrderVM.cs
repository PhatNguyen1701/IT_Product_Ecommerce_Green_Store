using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; } = "";
        public DateTime? OrderDate { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? ReceiverName { get; set; }
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string TypeShipping { get; set; } = "";
        public double ShippingFee { get; set; }
        public int StatusId { get; set; }
        public string? StaffId { get; set; } = "";
        public string? Note { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
    }
}
