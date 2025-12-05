namespace Spice_Cloud.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? PaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
