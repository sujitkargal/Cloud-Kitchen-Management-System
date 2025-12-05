namespace Spice_Cloud.Models
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        // Price after applying discount
        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; }
    }
}