namespace Spice_Cloud.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

   
        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public int Quantity { get; set; }

        // Original dish price
        public decimal Price { get; set; }

        // Discount percentage
        public decimal Discount { get; set; }

        // Price after discount for this quantity
        public decimal FinalPrice { get; set; }
    }
}
