namespace ITProductECommerce.Data
{
    public class Order
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

        public Customer Customer { get; set; }
        public Status Status { get; set; }
        public Staff Staff { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
