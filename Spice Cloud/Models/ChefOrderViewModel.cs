using System;
using System.Collections.Generic;

namespace Spice_Cloud.Models
{
    public class ChefOrderViewModel
    {
        public int OrderId { get; set; }
        public string RazorpayOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
