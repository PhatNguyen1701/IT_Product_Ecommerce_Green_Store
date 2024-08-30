using ITProductECommerce.Data;

namespace ITProductECommerce.ViewModels
{
    public class OrderDetailVM
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ReceiverName { get; set; }
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string TypeShipping { get; set; } = "";
        public double ShippingFee { get; set; }
        public int StatusId { get; set; }
        public string? StaffId { get; set; } = "";
        public string? Note { get; set; }
        public double Total => Quantity * Price * (Discount / 100);
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
    }
}
